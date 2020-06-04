using System;
using System.Collections.Generic;
using System.Diagnostics;

internal struct StoredData
{
    internal Process proc;
    internal bool proc_capacity;
    internal IntPtr mod_addr;
    internal IntPtr asmp;
    internal List<string> libs;
}