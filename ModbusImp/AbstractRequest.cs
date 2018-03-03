namespace ModbusImp
{
    public abstract class Request
    {
        protected const ushort maxLengthMsg = 256;
        public int Header { get; protected set; }
        protected byte slaveId;
        protected byte functionCode;
        protected int dataLength;
        protected byte[] requestData;
        public byte[] RequestMsg { get; protected set; }
        public int ExpectedBytes { get; protected set; }
        protected abstract void Build(byte[] data);
        public abstract int GetMsgLenth();

        protected Request(byte slaveId, byte functionCode, byte[] requestData)
        {
            this.slaveId = slaveId;
            this.functionCode = functionCode;
            this.requestData = requestData;
            dataLength = requestData.Length;
        }
    }
}
