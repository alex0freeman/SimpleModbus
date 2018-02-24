using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ModbusImp
{
    abstract class Request
    {
        protected const ushort maxLengthMsg = 256;
        protected object requestPack;
        protected int header { get; set; }
        protected int crcLength;
        public byte[] RequestMsg { get; protected set; }
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

    class TCPRequest : Request
    {
        private ushort protocolId = 0;
        private int StartDataIndex;
        private int length;
        public static ushort transactionId { get; private set; } = 0;

        public TCPRequest(byte slaveId, byte functionCode, ushort startAddress, ushort readCnt) : base (slaveId, functionCode, startAddress, readCnt)
        {
            header = 6;
            StartDataIndex = header;
            RequestMsg = new byte[header + BytesCnt()];  // [+byteSequenceCnt]
            transactionId++;
            Console.WriteLine(RequestMsg.Length);
            length = BytesCnt();
            Build();
        }

        protected override void Build()
        {
            RequestMsg[0] = (byte)(transactionId >> 8);
            RequestMsg[1] = (byte)(transactionId);
            RequestMsg[2] = (byte)(protocolId >> 8);
            RequestMsg[3] = (byte)(protocolId);
            RequestMsg[4] = (byte)(length >> 8);
            RequestMsg[5] = (byte)(length);
            Array.Copy(GetBytes(), 0, RequestMsg, StartDataIndex, length);
            Console.WriteLine(BitConverter.ToString(RequestMsg));
        }

        public override int GetMsgLenth()
        {
            return RequestMsg.Length;
        }
    }
}
