using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using asm = Assembler;
using mem = Memory;

internal static class Injector
{
    private static readonly Dictionary<string, IntPtr> exports = new Dictionary<string, IntPtr>() {
        {"mono_assembly_close", IntPtr.Zero},
        {"mono_assembly_get_image", IntPtr.Zero},
        {"mono_assembly_load_from_full", IntPtr.Zero},
        {"mono_class_from_name", IntPtr.Zero},
        {"mono_class_get_method_from_name", IntPtr.Zero},
        {"mono_get_root_domain", IntPtr.Zero},
        {"mono_image_open_from_data", IntPtr.Zero},
        {"mono_runtime_invoke", IntPtr.Zero},
        {"mono_thread_attach", IntPtr.Zero},
    };
    private const uint WAIT_FAILED = 0xffffffff;

    internal const uint ERROR_MONO_FAILED = 0x1;
    internal const uint ERROR_CLASS_NOT_FOUND = 0x2;
    internal const uint ERROR_METHOD_NOT_FOUND = 0x3;
    internal const uint ERROR_INVOKE_FAILED = 0x4;

    private static Process proc;
    private static IntPtr root_domain;
    private static bool proc_extended;
    private static bool attach;

    internal static IntPtr asmp = IntPtr.Zero; /* The current (previous) assembly pointer. */

    private static void GetRootDomain(ref IntPtr root_domain) {
        Execute(Injector.exports["mono_get_root_domain"], ref root_domain);

        AssertZero(root_domain, "GetRootDomain");
    }

    private static void OpenImageFromData(byte[] bytes, ref IntPtr img_raw) {
        Execute(Injector.exports["mono_image_open_from_data"], ref img_raw, mem.AllocAndWrite(bytes),
            (IntPtr) bytes.Length, (IntPtr) 1, IntPtr.Zero);

        AssertZero(img_raw, "OpenImageFromData");
    }

    private static void OpenAssemblyFromImage(IntPtr img_raw, ref IntPtr asmp) {
        Injector.attach = true;

        if (Injector.asmp != IntPtr.Zero)
            CloseAssembly(Injector.asmp);

        Execute(Injector.exports["mono_assembly_load_from_full"], ref asmp, img_raw,
            mem.AllocAndWrite(new byte[1]), IntPtr.Zero, IntPtr.Zero);

        AssertZero(asmp, "OpenAssemblyFromImage");
    }

    private static void GetImageFromAssembly(IntPtr asmp, ref IntPtr img) {
        Execute(Injector.exports["mono_assembly_get_image"], ref img, asmp);

        AssertZero(img, "GetImageFromAssembly");
    }

    private static void GetClassFromName(IntPtr img, ref IntPtr clp, string lib_n, string lib_c) {
        Execute(Injector.exports["mono_class_from_name"], ref clp, img,
            mem.AllocAndWrite(Encoding.UTF8.GetBytes(lib_n)),
            mem.AllocAndWrite(Encoding.UTF8.GetBytes(lib_c)));
    }

    private static void GetMethodFromName(IntPtr clp, ref IntPtr mp, string lib_m) {
        Execute(Injector.exports["mono_class_get_method_from_name"], ref mp, clp,
            mem.AllocAndWrite(Encoding.UTF8.GetBytes(lib_m)), IntPtr.Zero);
    }

    private static void RuntimeInvoke(IntPtr mp, ref IntPtr invoke_status) {
        Execute(Injector.exports["mono_runtime_invoke"], ref invoke_status, mp, IntPtr.Zero,
            IntPtr.Zero, IntPtr.Zero);
    }

    private static void CloseAssembly(IntPtr asmp) {
        IntPtr unused = IntPtr.Zero;

        Execute(Injector.exports["mono_assembly_close"], ref unused, asmp);
    }

    private static void Execute(IntPtr mono_addr, ref IntPtr exit_code, params IntPtr[] args) {
        IntPtr t;
        IntPtr addr = mem.AllocAndWrite(Assemble(mono_addr, args));

        t = Native.CreateRemoteThread(Injector.proc.Handle, IntPtr.Zero, 0, addr, IntPtr.Zero,
            ThreadCreationFlags.CREATE_SUSPENDED, out uint _);
        if (t == IntPtr.Zero)
            Utils.Assert("CreateRemoteThread");

        if (Native.ResumeThread(t) == uint.MaxValue)
            Utils.Assert("ResumeThread");

        if (Native.WaitForSingleObject(t, -1) == WAIT_FAILED)
            Utils.Assert("WaitForSingleObject");

        if (!Native.GetExitCodeThread(t, out exit_code))
            Utils.Assert("GetExitCodeThread");

        Memory.FreeAddr(addr);
        Native.CloseHandle(t);
    }

    private static byte[] Assemble(IntPtr addr, IntPtr[] args) {
        asm.Clean();

        if (Injector.proc_extended) {
            asm.SubRsp(0x28);

            if (Injector.attach) {
                asm.MovRax(Injector.exports["mono_thread_attach"]);
                asm.MovRcx(Injector.root_domain);
                asm.CallRax();
            }

            asm.MovRax(addr);

            for (int i = 0; i < args.Length; i++) {
                switch (i) {
                case 0:
                    asm.MovRcx(args[i]);

                    break;
                case 1:
                    asm.MovRdx(args[i]);

                    break;
                case 2:
                    asm.MovR8(args[i]);

                    break;
                case 3:
                    asm.MovR9(args[i]);

                    break;
                }
            }

            asm.CallRax();
            asm.AddRsp(0x28);
        } else {
            if (Injector.attach) {
                asm.Push(Injector.root_domain);
                asm.MovEax(Injector.exports["mono_thread_attach"]);
                asm.CallEax();
                asm.AddEsp(0x4);
            }

            for (int i = args.Length - 1; i >= 0; i--)
                asm.Push(args[i]);

            asm.MovEax(addr);
            asm.CallEax();
            asm.AddEsp((byte) (args.Length * 4));
        }

        asm.Return();

        return asm.ToByteArray();
    }

    private static bool GetExports(IntPtr addr) {
        /* Walk through the PE format. You can find IMAGE_* in the winnt.h header. */

        const int HFILE_SIZE = 20; /* sizeof(IMAGE_FILE_HEADER). */
        const int DD32_OFFSET = 96; /* sizeof(IMAGE_OPTIONAL_HEADER32) - sizeof(hopt->DataDirectory). */
        const int DD64_OFFSET = 112; /* sizeof(IMAGE_OPTIONAL_HEADER64) - sizeof(hopt->DataDirectory). */

        /* ((IMAGE_DOS_HEADER *) addr)->e_lfanew. 
         * (The each PE file starts with this header.) */
        int e_lfanew;

        IntPtr hfile; /* The pointer to the IMAGE_FILE_HEADER. */
        IntPtr hopt; /* The pointer to the IMAGE_OPTIONAL_HEADER. */
        IntPtr dd; /* ((IMAGE_OPTIONAL_HEADER *) hopt)->DataDirectory. */
        IntPtr edata; /* The pointer to the IMAGE_EXPORT_DIRECTORY. */
        int fq; /* edata->NumberOfFunctions. */
        IntPtr pfuncs; /* edata->AddressOfFunctions. */
        IntPtr pnames; /* edata->AddressOfNames. */
        IntPtr pnords; /* edata->AddressOfNameOrdinals. */

        e_lfanew = mem.Read(addr + 0x3c, 4);

        hfile = addr + e_lfanew + sizeof(uint);
        hopt = hfile + HFILE_SIZE;
        dd = hopt + (Injector.proc_extended ? DD64_OFFSET : DD32_OFFSET);
        edata = addr + mem.Read(dd, 4); /* Rude, but shorter. */

        fq = mem.Read(edata + 0x14, 4);
        pfuncs = addr + mem.Read(edata + 0x1c, 4);
        pnames = addr + mem.Read(edata + 0x20, 4);
        pnords = addr + mem.Read(edata + 0x24, 4);

        for (int i = 0; i < fq; i++) {
            int ord = mem.Read(pnords + i * sizeof(short), 2) * sizeof(int);
            IntPtr faddr = addr + mem.Read(pfuncs + ord, 4);
            string fname = mem.ReadStr(addr + mem.Read(pnames + ord, 4), 32);

            if (Injector.exports.ContainsKey(fname))
                Injector.exports[fname] = faddr;
        }

        return Injector.exports.All(export => export.Value != IntPtr.Zero);
    }

    private static void AssertZero(IntPtr ptr, string func_name) {
        if (ptr == IntPtr.Zero)
            throw new ApplicationException(func_name + "() returned zero pointer.");
    }

    internal static uint Inject(InjectData data) {
        byte[] bytes = File.ReadAllBytes(data.lib_path);
        IntPtr img_raw = IntPtr.Zero;
        IntPtr img = IntPtr.Zero;
        IntPtr clp = IntPtr.Zero;
        IntPtr mp = IntPtr.Zero;
        IntPtr invoke_status = IntPtr.Zero;

        Injector.proc = data.proc;
        Injector.proc_extended = data.proc_extended;
        Injector.asmp = data.asmp;
        Injector.attach = false;

        mem.Update(Injector.proc.Handle);

        if (!GetExports(data.mod_addr))
            return ERROR_MONO_FAILED;

        GetRootDomain(ref Injector.root_domain);
        OpenImageFromData(bytes, ref img_raw);
        OpenAssemblyFromImage(img_raw, ref Injector.asmp);
        GetImageFromAssembly(Injector.asmp, ref img);

        GetClassFromName(img, ref clp, data.lib_n, data.lib_c);
        if (clp == IntPtr.Zero)
            return ERROR_CLASS_NOT_FOUND;

        GetMethodFromName(clp, ref mp, data.lib_m);
        if (mp == IntPtr.Zero)
            return ERROR_METHOD_NOT_FOUND;

        RuntimeInvoke(mp, ref invoke_status);
        if (invoke_status != IntPtr.Zero)
            return ERROR_INVOKE_FAILED;

        return 0;
    }

    internal static void Eject(StoredData data, string lib_n, string lib_c, string lib_m) {
        IntPtr img = IntPtr.Zero;
        IntPtr clp = IntPtr.Zero;
        IntPtr mp = IntPtr.Zero;
        IntPtr invoke_status = IntPtr.Zero;

        Injector.asmp = data.asmp;
        Injector.attach = true;

        GetImageFromAssembly(Injector.asmp, ref img);
        GetClassFromName(img, ref clp, lib_n, lib_c);
        GetMethodFromName(clp, ref mp, lib_m);
        RuntimeInvoke(mp, ref invoke_status);
        CloseAssembly(Injector.asmp);
    }
}