using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ModbusImp
{
    interface MBContext
    {
        void Connect();
        void Disconnect();
        int SendMsg(byte[] msg);
        int RecieveMsg(ref byte[] buff);
       // byte[] BuildMessage(RequestPacket request);
    }

    class TCPContex : MBContext
    {
        IPAddress Ip;
        ushort Port;
        private Socket tcpSocket;
        Request tcpRequest;

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


        //byte[] MBContext.BuildMessage(RequestPacket request)
        //{
        //    tcpRequest = new TCPRequest(request);
        //    return tcpRequest.RequestMsg;
        //}

    }

    public class Transport<T>
    {
        private Transport() { }

        static readonly Dictionary<int, Func<T>> _dict
             = new Dictionary<int, Func<T>>();

        public static T Create(int id)
        {
            Func<T> constructor = null;
            if (_dict.TryGetValue(id, out constructor))
                return constructor();

            throw new ArgumentException("No type registered for this id");
        }

        public static void Register(int id, Func<T> ctor)
        {
            _dict.Add(id, ctor);
        }
    }




}
