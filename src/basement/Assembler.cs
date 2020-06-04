using System;
using System.Collections.Generic;

internal static class Assembler
{
    private static readonly List<byte> asm = new List<byte>();

    internal static void MovRax(IntPtr arg) {
        Assembler.asm.AddRange(new byte[2] {
            0x48,
            0xb8
        });

        Assembler.asm.AddRange(BitConverter.GetBytes((long) arg));
    }

    internal static void MovRcx(IntPtr arg) {
        Assembler.asm.AddRange(new byte[2] {
            0x48,
            0xb9
        });

        Assembler.asm.AddRange(BitConverter.GetBytes((long) arg));
    }

    internal static void MovRdx(IntPtr arg) {
        Assembler.asm.AddRange(new byte[2] {
            0x48,
            0xba
        });

        Assembler.asm.AddRange(BitConverter.GetBytes((long) arg));
    }

    internal static void MovR8(IntPtr arg) {
        Assembler.asm.AddRange(new byte[2] {
            0x49,
            0xb8
        });

        Assembler.asm.AddRange(BitConverter.GetBytes((long) arg));
    }

    internal static void MovR9(IntPtr arg) {
        Assembler.asm.AddRange(new byte[2] {
            0x49,
            0xb9,
        });

        Assembler.asm.AddRange(BitConverter.GetBytes((long) arg));
    }

    internal static void SubRsp(byte arg) {
        Assembler.asm.AddRange(new byte[3] {
            0x48,
            0x83,
            0xec
        });

        Assembler.asm.Add(arg);
    }

    internal static void CallRax() {
        Assembler.asm.AddRange(new byte[2] {
            0xff,
            0xd0
        });
    }

    internal static void AddRsp(byte arg) {
        Assembler.asm.AddRange(new byte[3] {
            0x48,
            0x83,
            0xc4
        });

        Assembler.asm.Add(arg);
    }

    internal static void MovEax(IntPtr arg) {
        Assembler.asm.Add(0xb8);
        Assembler.asm.AddRange(BitConverter.GetBytes((int) arg));
    }

    internal static void CallEax() {
        Assembler.asm.AddRange(new byte[2] {
            0xff,
            0xd0
        });
    }

    internal static void AddEsp(byte arg) {
        Assembler.asm.AddRange(new byte[2] {
            0x83,
            0xc4
        });

        Assembler.asm.Add(arg);
    }

    internal static void Push(IntPtr arg) {
        byte[] num_arr = new byte[1] {
            (byte) (int) arg
        };

        if ((int) arg > byte.MaxValue)
            num_arr = BitConverter.GetBytes((int) arg);

        Assembler.asm.Add((int) arg < 0x80 ? (byte) 0x6a : (byte) 0x68);
        Assembler.asm.AddRange(num_arr);
    }

    internal static void Return() {
        Assembler.asm.Add(0xc3);
    }

    internal static byte[] ToByteArray() {
        return Assembler.asm.ToArray();
    }

    internal static void Clean() {
        Assembler.asm.Clear();
    }
}