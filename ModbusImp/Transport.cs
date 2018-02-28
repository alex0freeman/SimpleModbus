using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ModbusImp
{
    public class TCPContex : MBContext
    {
        IPAddress Ip;
        ushort Port;
        private Socket tcpSocket;
        TCPRequest tcpRequest;
        TCPResponce tcpResponce;
        int expectedResponceBytes;


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

        byte[] MBContext.BuildMessage(byte slaveId, byte functionCode, ushort startAddress, ushort readCnt)
        {
            tcpRequest = new TCPRequest(slaveId, functionCode, startAddress, readCnt);
            return tcpRequest.RequestMsg;
        }

    

        byte[] MBContext.GetContent(byte[] fullResponce)
        {
            tcpResponce = new TCPResponce(fullResponce, tcpRequest.ExpectedBytes);
            
            return tcpResponce.data;
        }


    }

 
}
