using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ModbusImp
{
    static class TypeManager<T> where T : struct
    {
        public static byte[] ToBytes(T type)
        {
            int size = Marshal.SizeOf(type);
            byte[] array = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(type, ptr, true);
            Marshal.Copy(ptr, array, 0, size);
            Marshal.FreeHGlobal(ptr);
            return array;
        }

        public static int GetExpectedBytesByFunction(int function, int elementsCnt)
        {

            switch (function)
            {
                case ((byte)MbFunctions.ReadCoils):
                case ((byte)MbFunctions.ReadInputs):
                    return 1 + ((elementsCnt >= 8) ? (elementsCnt / 8 + 1) : 1);
                case ((byte)MbFunctions.ReadHoldingRegisters):
                case ((byte)MbFunctions.ReadInputRegister):
                    return 1 + elementsCnt * sizeof(short);

                default:
                    return 0;
            }
        }

    }
}
