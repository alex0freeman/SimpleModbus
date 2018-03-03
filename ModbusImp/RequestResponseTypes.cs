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

        public MBReadResponse(byte[] request)
        {
            slaveId = request[0];
            functionId = request[1];
            nextBytesCnt = request[2];
            readCnt = new byte[nextBytesCnt];
            Array.Copy(request, 3, readCnt, 0, request.Length - 3);
        }
    }
}
