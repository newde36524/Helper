using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Helper
{
    public class MarshalHelper
    {
        public static IEnumerable<byte> HGlobalToBytes(IntPtr intPtr, int len)
        {
            for (int i = 0; i < len; i++)
            {
                yield return Marshal.ReadByte(intPtr, i);
            }
        }
        public static IntPtr BytesToHGlobal(byte[] source)
        {
            byte[] temp = source;
            IntPtr result = Marshal.AllocHGlobal(temp.Length);
            for (int i = 0; i < temp.Length; i++)
            {
                Marshal.WriteByte(result, i, temp[i]);
            }
            return result;
        }

        public static IntPtr StructureToHGlobal<T>(T structure, bool fDeleteOld = true) where T : struct
        {
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, ptr, fDeleteOld);
            return ptr;
        }

    }
}
