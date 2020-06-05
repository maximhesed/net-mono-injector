using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

internal static class Memory
{
    private static readonly Dictionary<IntPtr, uint> allocs = new Dictionary<IntPtr, uint>();

    internal static IntPtr Handle {
        get;
        set;
    }

    internal static void Dump(string file_name) {
        FileStream fs = File.Open(file_name, FileMode.OpenOrCreate);
        StringBuilder sb = new StringBuilder();

        foreach (KeyValuePair<IntPtr, uint> item in Memory.allocs) {
            IntPtr addr = item.Key;
            uint size = item.Value;

            sb.AppendFormat("{0}: {1}\n", addr.ToString("X8"), size);
        }

        fs.Write(Encoding.ASCII.GetBytes(sb.ToString()), 0, sb.Length);
        fs.Flush();
        fs.Close();
    }

    internal static int Read(IntPtr addr, uint size) {
        byte[] buf = new byte[size];
        string value = "";

        if (!Native.ReadProcessMemory(Memory.Handle, addr, buf, size, out uint _))
            Utils.Assert("ReadProcessMemory");

        foreach (byte b in buf.Reverse())
            value += b.ToString("x2");

        return Convert.ToInt32(value, 0x10);
    }

    internal static string ReadStr(IntPtr addr, int length) {
        string str = "";

        for (int i = 0; i < length; i++) {
            char c;

            c = (char) Memory.Read(addr + i, 1);
            if (c == 0)
                break;

            str += c;
        }

        return str;
    }

    internal static IntPtr Alloc(uint size) {
        IntPtr addr;

        addr = Native.VirtualAllocEx(Memory.Handle, IntPtr.Zero, size, AllocationType.MEM_COMMIT,
            MemoryProtection.PAGE_EXECUTE);
        if (addr == IntPtr.Zero)
            Utils.Assert("VirtualAllocEx");

        Memory.allocs.Add(addr, size);

        return addr;
    }

    internal static void Write(IntPtr addr, byte[] data) {
        if (!Native.WriteProcessMemory(Memory.Handle, addr, data, (uint) data.Length, out uint _))
            Utils.Assert("WriteProcessMemory");
    }

    internal static IntPtr AllocAndWrite(byte[] data) {
        IntPtr addr = Memory.Alloc((uint) data.Length);

        Memory.Write(addr, data);

        return addr;
    }

    internal static void FreeAddr(IntPtr addr) {
        if (!Native.VirtualFreeEx(Memory.Handle, addr, 0, MemoryFreeType.MEM_RELEASE))
            Utils.Assert("VirtualFreeEx");

        Memory.allocs.Remove(addr);
    }

    internal static void Free() {
        for (int i = 0; i < Memory.allocs.Count; i++)
            Memory.FreeAddr(Memory.allocs.ElementAt(i).Key);
    }

    internal static void Update(IntPtr h) {
        Memory.allocs.Clear();

        Memory.Handle = h;
    }
}