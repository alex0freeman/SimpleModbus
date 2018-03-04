using System;
using System.Collections.Generic;

namespace ModbusImp
{
    public interface IMBContext
    {
        void Connect();
        void Disconnect();
        int SendMsg(byte[] msg);
        int RecieveMsg(ref byte[] buff);
        byte[] BuildMessage(byte slaveId, byte functionCode, byte[] data);
        byte[] GetContent(byte[] fullResponse, int expectedBytes);
        int GetHeader(); 
    }
}
