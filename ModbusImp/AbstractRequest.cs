using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ModbusImp
{
    public abstract class Request
    {
        protected const ushort maxLengthMsg = 256;
        protected object requestPack;
        protected int header;
        protected int crcLength;
        public byte[] RequestMsg { get; protected set; }
        public int ExpectedBytes { get; protected set; }
        protected int responceExectedHeader = 3;
        protected abstract void Build();
        public abstract int GetMsgLenth();

        protected Request(byte slaveId, byte functionCode, ushort startAddress, ushort readCnt)
        {
            requestPack = new MBRequestReadWriteOne(slaveId, functionCode, startAddress, readCnt);
        }

        protected Request(byte slaveId, byte functionCode, ushort startAddress, ushort readCnt, byte byteSequenceCnt)
        {
            requestPack = new MBRequestWrite(slaveId, functionCode, startAddress, readCnt, byteSequenceCnt);
        }

        protected int GetExpectedBytesByFunction(byte functionCode, int elementsCnt)
        {
            switch (functionCode)
            {
                case ((byte)MbFunctions.ReadCoils):
                case ((byte)MbFunctions.ReadInputs):
                    return (elementsCnt >= 8) ? (elementsCnt / 8 + 1) : 1;
                case ((byte)MbFunctions.ReadHoldingRegisters):
                case ((byte)MbFunctions.ReadInputRegister):
                    return elementsCnt * sizeof(short);
                default:
                    return 0;
            }

        }

        protected byte[] GetBytes()
        {
            int size = Marshal.SizeOf(requestPack);
            byte[] array = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(requestPack, ptr, true);
            Marshal.Copy(ptr, array, 0, size);
            Marshal.FreeHGlobal(ptr);
            return array;
        }

        protected int BytesCnt()
        {
            return GetBytes().Length + crcLength;
        }
    }
}
