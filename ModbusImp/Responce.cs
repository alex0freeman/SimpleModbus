using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusImp
{
    abstract class Responce
    {
        public ushort expectedResponce { get; protected set; }
        protected byte slaveId { get; set; }
        protected byte functionNum { get; set; }
        protected const ushort maxLengthMsg = 256;
        protected ushort dataLength { get; set; }
        protected byte[] data { get; set; }
        protected abstract void TryParse(byte[] responce);
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


    //class TCPResponce : Responce
    //{
    //    const int mbapHeader = 8;

    //    public TCPResponce(byte[] responce, int expected)
    //    {
    //        if (responce.Length != expected) 
    //        {
    //            var Error = new ErrorHandling(responce, 8);
    //        }
    //        else
    //        {

    //        }
    //    }

    //    protected override void TryParse(byte[] responce, out byte[] result)
    //    {

    //    }
    //  }
}
