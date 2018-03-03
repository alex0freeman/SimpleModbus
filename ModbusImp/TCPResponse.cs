using System;

namespace ModbusImp
{
    class TCPResponse : Response
    {
        const int mbapHeader = 6;
        
        public TCPResponse(byte[] response, int expected)
        {
            byte[] message = new byte[response.Length - mbapHeader];
            Array.Copy(response, mbapHeader, message, 0, message.Length);

            if (response.Length != expected)
            {
                Console.WriteLine("{0} {1}", response.Length, expected);
                var Error = new ErrorHandling(response, 8);
            }
            else
            {
                TryParse(message, out data);
            }
        }

        protected override void TryParse(byte[] response, out byte[] result)
        {
            MBReadResponse res = new MBReadResponse(response);
            result = res.readCnt;
        }
    }
}
