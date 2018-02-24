
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ModbusImp
{
    class MainApp
    {
        public static void Main()
        {
            TCPRequest ttcp = new TCPRequest(1,3,3,2);
            //Transport<MBContext>.Register(1, () => new TCPContex("192.168.0.111", 502));
            //MBContext tcp = Transport<MBContext>.Create(1);

            //tcp.Connect();
            //ModbusDevice<MBContext> mb = new ModbusDevice<MBContext>(tcp);
            //for (ushort i = 0; i < 32; i++)
            //{
            //    Console.Write("Coil: ");
            //    mb.ReadCoils(i, 1);
            //}

            ////tcp.Disconnect();
            Console.ReadLine();
        }
    }
}
