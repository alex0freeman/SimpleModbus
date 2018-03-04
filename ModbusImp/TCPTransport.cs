using System;
using System.Net;
using System.Net.Sockets;

namespace ModbusImp
{
    /// <summary>
    /// Modbus TCP device context
    /// </summary>
    public class TCPContext : IMBContext
    {
        /// <summary>
        /// IP address of Modbus device
        /// </summary>
        private IPAddress _ip;

        /// <summary>
        /// Modbus port
        /// </summary>
        private ushort _port;

        /// <summary>
        /// TCP socket used in connection
        /// </summary>
        private Socket _tcpSocket;
        
        private TCPRequest _tcpRequest;
        private TCPResponse _tcpResponse;
        
        public TCPContext(string ip, ushort port)
        {
            if (!IPAddress.TryParse(ip, out _ip))
            {
                throw new Exception("Invalid IP address");
            }
            _port = port;
        }

        string IMBContext.ConnectionCredentials()
        {
            return _ip + ":" + _port;
        }

        void IMBContext.Connect()
        {
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(_ip, _port);
                _tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _tcpSocket.Connect(remoteEP);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        void IMBContext.Disconnect()
        {
            _tcpSocket.Shutdown(SocketShutdown.Both);
            _tcpSocket.Close();
        }

        int IMBContext.SendMsg(byte[] message)
        {
            int bytesSent = _tcpSocket.Send(message);
            return bytesSent;
        }

        int IMBContext.RecieveMsg(ref byte[] buff)
        { 
            int bytesRec = _tcpSocket.Receive(buff);
            return bytesRec;
        }

        byte[] IMBContext.BuildMessage(byte slaveId, byte functionCode, byte[] data)
        {
            _tcpRequest = new TCPRequest(slaveId, functionCode, data);
            return _tcpRequest.RequestMsg;
        }

        byte[] IMBContext.GetContent(byte[] fullResponse, int expectedBytes)
        {
            _tcpResponse = new TCPResponse(fullResponse, expectedBytes);
            
            return _tcpResponse.data;
        }

        int IMBContext.GetHeader()
        {
            return _tcpRequest.Header;
        }
    }
}
