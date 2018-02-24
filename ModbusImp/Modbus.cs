using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ModbusImp
{
    enum MbErrors
    {
        IllegalFunction = 1,
        IllegalDataAddress,
        IllegalDataValue,
        SlaveDeviceFailure,
        Acknowlegment,
        SlaveDeviceBusy,
        NegativeAcknowledge,
        MemoryParityError,
        GatewayPathUnavailable,
        GatewayTargetdeviceFailedToRespond
    };

    enum MbFunctions
    {
        ReadCoils = 1,
        ReadInputs,
        ReadHoldingRegisters,
        ReadInputRegister
    };

    class ModbusDevice<T> where T : MBContext
    {
        private T cntx; // Modbus context

        public ModbusDevice(T cntx)
        {
            this.cntx = cntx;
        }

        // Read data from current context
        private int Read(byte funcNumber, ushort startAddress, ushort numItems)
        {
            byte[] buff = new byte[256]; // Buffer to save response

            int cnt = cntx.RecieveMsg(ref buff);
            Console.WriteLine(BitConverter.ToString(buff, 0, cnt));

            return 0;
        }

        public int ReadCoils(ushort startAddress, ushort numItems)
        {
            return Read((byte)MbFunctions.ReadCoils, startAddress, numItems);
        }

        public int ReadInput(ushort startAddress, ushort numItems)
        {
            return Read((byte)MbFunctions.ReadInputs, startAddress, numItems);
        }


        public byte[] ToByte<U>(U str) where U : struct
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
    }
}
