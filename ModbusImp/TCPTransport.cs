using System;
using System.Net;
using System.Net.Sockets;

namespace ModbusImp
{
    public class TCPContex : MBContext
    {
        IPAddress Ip;
        ushort Port;
        private Socket tcpSocket;
        TCPRequest tcpRequest;
        TCPResponse _tcpResponse;

        public TCPContex(string ip, ushort port)
        {
            if (!IPAddress.TryParse(ip, out Ip))
            {
                throw new Exception("Invalid IP address");
            }
            Port = port;
        }

        void MBContext.Connect()
        {
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(Ip, Port);
                tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                tcpSocket.Connect(remoteEP);
                Console.WriteLine("Connected");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        void MBContext.Disconnect()
        {
            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();
        }

        int MBContext.SendMsg(byte[] message)
        {
            int bytesSent = tcpSocket.Send(message);
            return bytesSent;
        }

        int MBContext.RecieveMsg(ref byte[] buff)
        { 
            int bytesRec = tcpSocket.Receive(buff);
            return bytesRec;
        }

        byte[] MBContext.BuildMessage(byte slaveId, byte functionCode, byte[] data)
        {
            tcpRequest = new TCPRequest(slaveId, functionCode, data);
            return tcpRequest.RequestMsg;
        }

        byte[] MBContext.GetContent(byte[] fullResponse, int expectedBytes)
        {
            _tcpResponse = new TCPResponse(fullResponse, expectedBytes);
            
            return _tcpResponse.data;
        }

        int MBContext.GetHeader()
        {
            return tcpRequest.Header;
        }
    }
}
