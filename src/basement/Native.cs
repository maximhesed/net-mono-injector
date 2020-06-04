using System;
using System.Runtime.InteropServices;
using System.Text;

internal static class Native
{
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool IsWow64Process(IntPtr h, out bool extended);

    [DllImport("psapi.dll")]
    internal static extern bool EnumProcessModulesEx(IntPtr h,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4), In, Out] IntPtr[] p,
        uint cb, [MarshalAs(UnmanagedType.U4)] out uint size, ModuleFilter filter_flag);

    [DllImport("psapi.dll")]
    internal static extern uint GetModuleFileNameEx(IntPtr h, IntPtr mod_h,
        [Out] StringBuilder base_name, [MarshalAs(UnmanagedType.U4), In] uint nSize);

    [DllImport("psapi.dll", SetLastError = true)]
    internal static extern bool GetModuleInformation(IntPtr h, IntPtr mod_h, out ModuleInfo mod_info,
        uint cb);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    internal static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPTStr), In] string file_name);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool FreeLibrary(IntPtr mod_h);

    [DllImport("kernel32.dll")]
    internal static extern IntPtr GetProcAddress(IntPtr mod_h,
        [MarshalAs(UnmanagedType.LPStr), In] string proc_name);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool WriteProcessMemory(IntPtr h, IntPtr base_addr, byte[] buf, uint nsize,
        out uint bytes_written);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool ReadProcessMemory(IntPtr h, IntPtr base_addr, byte[] buf, uint nsize,
        out uint bytes_num);

    [DllImport("kernel32.dll")]
    internal static extern IntPtr VirtualAllocEx(IntPtr h, IntPtr addr, uint size,
        AllocationType alloc_type, MemoryProtection protect_flag);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool VirtualFreeEx(IntPtr h, IntPtr addr, uint size, MemoryFreeType mem_type);

    [DllImport("kernel32.dll")]
    internal static extern IntPtr CreateRemoteThread(IntPtr h, IntPtr thread_attrs,
        int stack_size, IntPtr start_addr, IntPtr param, ThreadCreationFlags creat_flags, out uint tid);

    [DllImport("kernel32.dll")]
    internal static extern uint WaitForSingleObject(IntPtr h, int time);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetExitCodeThread(IntPtr thread_h, out IntPtr exit_code);

    [DllImport("kernel32.dll")]
    internal static extern uint GetLastError();

    [DllImport("kernel32.dll")]
    internal static extern bool CloseHandle(IntPtr h);

    [DllImport("kernel32.dll")]
    internal static extern uint FormatMessageA(uint flags, IntPtr source, uint msg_id, uint lang_id,
        out IntPtr buf, uint size, params IntPtr[] args);

    [DllImport("kernel32.dll")]
    internal static extern IntPtr LocalFree(IntPtr mem_h);

    [DllImport("kernel32.dll")]
    internal static extern uint ResumeThread(IntPtr thread_h);
}