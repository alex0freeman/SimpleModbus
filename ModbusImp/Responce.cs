using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusImp
{
    public abstract class Responce
    {
        public ushort expectedResponce { get; protected set; }
        protected const ushort maxLengthMsg = 256;
        protected ushort dataLength { get; set; }
        public byte[] data;
        protected abstract void TryParse(byte[] responce, out byte[] result);
        protected class ErrorHandling
        {
            bool errorExist = false;
            private void Handle(byte functionCode)
            {

                var errorCode = functionCode - 128;
                if (errorExist = Enum.IsDefined(typeof(MbErrors), errorCode))
                {
                    throw new Exception(Enum.GetName(typeof(MbErrors), errorCode));
                }
                else
                {
                    throw new Exception("Not defined exception code");
                }
            }

            internal ErrorHandling(byte[] responce, int minLength)
            {
                if (responce.Length > minLength)
                {
                    Handle(responce[minLength]);
                }
                else
                {
                    throw new Exception("Undefined Error");
                }
            }
        }
    }

    class TCPResponce : Responce
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
