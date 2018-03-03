using System;

namespace ModbusImp
{
    public abstract class Response
    {
        public ushort expectedResponse { get; protected set; }
        protected const ushort maxLengthMsg = 256;
        protected ushort dataLength { get; set; }
        public byte[] data;
        protected abstract void TryParse(byte[] response, out byte[] result);

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

            internal ErrorHandling(byte[] response, int minLength)
            {
                if (response.Length > minLength)
                {
                    Handle(response[minLength]);
                }
                else
                {
                    throw new Exception("Undefined Error");
                }
            }
        }
    }
}
