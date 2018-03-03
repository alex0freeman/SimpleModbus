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
        private int length;
        public static ushort transactionId { get; private set; } = 0;

        public TCPRequest(byte slaveId, byte functionCode, byte[] requestData) : base(slaveId, functionCode, requestData)
        {
            Header = 8;
            RequestMsg = new byte[Header + dataLength];
            transactionId++;
            length = 2 + dataLength;
     //       ExpectedBytes = header + GetExpectedBytesByFunction(functionCode);
            Build(requestData);
        }

        protected override void Build(byte[] data)
        {
            RequestMsg[0] = (byte)(transactionId >> 8);
            RequestMsg[1] = (byte)(transactionId);
            RequestMsg[2] = (byte)(protocolId >> 8);
            RequestMsg[3] = (byte)(protocolId);
            RequestMsg[4] = (byte)(length >> 8);
            RequestMsg[5] = (byte)(length);
            RequestMsg[6] = (slaveId);
            RequestMsg[7] = (functionCode);
            Array.Copy(data, 0, RequestMsg, Header, data.Length);
            Console.WriteLine(BitConverter.ToString(RequestMsg));
        }

        public override int GetMsgLenth()
        {
            return RequestMsg.Length;
        }
    }
}
