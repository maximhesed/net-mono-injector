using System;
using System.Diagnostics;

internal struct InjectData
{
    internal Process proc;
    internal bool proc_extended;
    internal IntPtr mod_addr;
    internal IntPtr asmp;
    internal string lib_path;
    internal string lib_n;
    internal string lib_c;
    internal string lib_m;
};