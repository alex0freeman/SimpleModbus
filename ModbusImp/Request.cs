using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ModbusImp
{
    public class TCPRequest : Request
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
            length = BytesCnt();
            Build();
            ExpectedBytes = header + responceExectedHeader + GetExpectedBytesByFunction(functionCode, readCnt);
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
         
        }

        public override int GetMsgLenth()
        {
            return RequestMsg.Length;
        }
    }
}
