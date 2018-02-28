using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusImp
{
  
    class TCPResponce : Response
    {
        const int mbapHeader = 6;
        public TCPResponce(byte[] responce, int expected)
        {
            byte[] message = new byte[responce.Length - mbapHeader];
            Array.Copy(responce, mbapHeader, message, 0, message.Length);

            if (responce.Length != expected)
            {
                Console.WriteLine("{0} {1}", responce.Length, expected);
                var Error = new ErrorHandling(responce, 8);
            }
            else
            {
                TryParse(message, out data);

            }
        }

        protected override void TryParse(byte[] responce, out byte[] result)
        {
            MBReadResponse res = new MBReadResponse(responce);
            result = res.readCnt;
        }
    }
}
