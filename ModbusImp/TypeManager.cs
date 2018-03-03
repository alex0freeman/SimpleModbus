﻿using System;
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
                case ((byte)MbFunctions.WriteMultiplyCoils):
                case ((byte)MbFunctions.WriteMultiplyHoldingRegisters):
                    return 3;
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