using System;
using System.Linq;
using System.Runtime.InteropServices;

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


        public static byte[] ToBytes(short[] array)
        {
            byte[] result = new byte[array.Length * sizeof(short)];
            Buffer.BlockCopy(array.Select(x => ReverseBytes(x)).ToArray(), 0, result , 0, result.Length);
            return result;
        }

        public static int GetExpectedBytesByFunction(int function, int elementsCnt)
        {
            switch (function)
            {
                case (byte)MbFunctions.ReadCoils:
                case (byte)MbFunctions.ReadDiscreteInputs:
                    return 1 + ((elementsCnt >= 8) ? (elementsCnt / 8 + 1) : 1);
                case (byte)MbFunctions.ReadHoldings:
                case (byte)MbFunctions.ReadInputs:
                    return 1 + elementsCnt * sizeof(short);
                case (byte)MbFunctions.WriteCoils:
                case (byte)MbFunctions.WriteHoldings:
                    return 4;
                default:
                    return 0;
            }
        }

        public static bool[] ParseDiscretes(byte[] responseData, int count)
        {
            bool[] discreteArray = new bool[count];
            for (int i = 0; i < count; i++)
            {
                int cur = (i >= 8) ? 0 : i;
                byte bitMask = (byte)(1 << cur);
                discreteArray[i] = Convert.ToBoolean(responseData[(i / 8)] & bitMask);
            }
            return discreteArray;
        }

        public static short[] ParseRegisters(byte[] responseData, int count)
        {
            short[] registersArray = new short[count];
            Buffer.BlockCopy(responseData, 0, registersArray, 0, responseData.Length);
            return registersArray.Select(x => ReverseBytes(x)).ToArray();
        }



        public static short ReverseBytes(short value)
        {
            return (short)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }
    }
}
