namespace ModbusImp
{
    /// <summary>
    /// Functions applicable to device context
    /// </summary>
    public interface IMBContext
    {
        /// <summary>
        /// Establish connection with target device
        /// </summary>
        void Connect();
        
        /// <summary>
        /// Disconnect from target device
        /// </summary>
        void Disconnect();
        
        /// <summary>
        /// Send message to target device
        /// </summary>
        /// <param name="msg">Message data</param>
        /// <returns>Number of bytes sent</returns>
        int SendMsg(byte[] msg);
        
        int RecieveMsg(ref byte[] buff);
        
        byte[] BuildMessage(byte slaveId, byte functionCode, byte[] data);
        
        byte[] GetContent(byte[] fullResponse, int expectedBytes);
        
        int GetHeader(); 
    }
}
