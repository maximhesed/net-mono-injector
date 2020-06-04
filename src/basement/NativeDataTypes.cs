using System;

[Flags]
internal enum AllocationType
{
    MEM_COMMIT = 0x1000,
    MEM_RESERVE = 0x2000,
    MEM_RESET = 0x80000,
    MEM_RESET_UNDO = 0x1000000,
    MEM_LARGE_PAGES = 0x2000000,
    MEM_PHYSICAL = 0x400000,
    MEM_TOP_DOWN = 0x100000
}

[Flags]
internal enum MemoryFreeType
{
    MEM_DECOMMIT = 0x4000,
    MEM_RELEASE = 0x8000
}

[Flags]
internal enum MemoryProtection
{
    PAGE_EXECUTE = 0x10,
    PAGE_EXECUTE_READ = 0x20,
    PAGE_EXECUTE_READWRITE = 0x40,
    PAGE_EXECUTE_WRITECOPY = 0x80,
    PAGE_NOACCESS = 0x1,
    PAGE_READONLY = 0x2,
    PAGE_READWRITE = 0x4,
    PAGE_WRITECOPY = 0x8,
    PAGE_TARGETS_INVALID = 0x40000000,
    PAGE_TARGETS_NO_UPDATE = 0x40000000,
    PAGE_GUARD = 0x100,
    PAGE_NOCACHE = 0x200,
    PAGE_WRITECOMBINE = 0x400
}

[Flags]
internal enum ThreadCreationFlags
{
    None = 0x0,
    CREATE_SUSPENDED = 0x4,
    STACK_SIZE_PARAM_IS_A_RESERVATION = 0x10000
}

internal enum ModuleFilter : uint
{
    LIST_MODULES_DEFAULT,
    LIST_MODULES_32BIT,
    LIST_MODULES_64BIT,
    LIST_MODULES_ALL,
}

internal struct ModuleInfo
{
    internal IntPtr dll_base;
    internal int img_size;
    internal IntPtr entry;
}