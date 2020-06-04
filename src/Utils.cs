using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

internal static class Utils
{
    private static readonly uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x100;
    private static readonly uint FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;
    private static readonly uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x200;

    internal const bool WIN32 = true;
    internal const bool WIN64 = false;

    internal static string GetLastErrorString() {
        uint err_code = Native.GetLastError();
        IntPtr msg_buf;
        string err_str;

        if (Native.FormatMessageA(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM |
                FORMAT_MESSAGE_IGNORE_INSERTS, IntPtr.Zero, err_code, 0, out msg_buf, 0, IntPtr.Zero) == 0)
            return err_code.ToString();

        err_str = Marshal.PtrToStringAnsi(msg_buf).TrimEnd('\n');

        Native.LocalFree(msg_buf);

        return err_str;
    }

    internal static IntPtr GetMonoModule(IntPtr h, bool proc_ext) {
        const uint ARR_SIZE = 512;
        const uint PATH_SIZE = 256;

        uint offset = proc_ext ? 8u : 4u;
        IntPtr[] mods_arr = new IntPtr[ARR_SIZE];
        uint size;
        uint length;

        if (Native.EnumProcessModulesEx(h, mods_arr, ARR_SIZE * offset, out size,
                ModuleFilter.LIST_MODULES_ALL)) {
            length = size / offset;

            for (int i = 0; i < length; i++) {
                StringBuilder base_name = new StringBuilder((int) PATH_SIZE);
                ModuleInfo mod_info;

                if (Native.GetModuleFileNameEx(h, mods_arr[i], base_name, PATH_SIZE) == 0)
                    return IntPtr.Zero;

                if (base_name.ToString().EndsWith("mono.dll", StringComparison.OrdinalIgnoreCase)) {
                    Native.GetModuleInformation(h, mods_arr[i], out mod_info, (uint) (offset * mods_arr.Length));

                    return mod_info.dll_base;
                }
            }
        }

        return IntPtr.Zero;
    }

    internal static bool GetProcExt(IntPtr h) {
        bool extended;

        if (!Environment.Is64BitOperatingSystem)
            return Utils.WIN32;

        if (!Native.IsWow64Process(h, out extended))
            throw new Win32Exception();

        if (extended)
            return Utils.WIN64;

        return Utils.WIN32;
    }

    internal static void Assert(string func_name) {
        throw new ApplicationException(func_name + "(): " + GetLastErrorString());
    }
}