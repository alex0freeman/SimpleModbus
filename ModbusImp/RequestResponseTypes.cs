using System;
using System.Runtime.InteropServices;

namespace ModbusImp
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBReadResponse
    {
        byte slaveId { get; set; }
        byte functionId { get; set; }
        byte nextBytesCnt { get; set; }
        public byte[] readCnt { get; set; }

        public MBReadResponse(byte[] response)
        {
            Console.WriteLine(BitConverter.ToString(response));
            slaveId = response[0];
            functionId = response[1];
            nextBytesCnt = response[2];
            readCnt = new byte[nextBytesCnt];
            Array.Copy(response, 3, readCnt, 0, response.Length - 3);
        }
    }

}