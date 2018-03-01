using ModbusImp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ModbusImp
{
    class Program
    {
        static void Main(string[] args)
        {
            Transport<TCPContex>.Register(1, () => new TCPContex("192.168.30.60", 502));
            TCPContex tcp = Transport<TCPContex>.Create(1);

            ModbusDevice<MBContext> tcpDevice = new ModbusDevice<MBContext>(tcp, 1);
            tcpDevice.Connect();
            bool[] coils = tcpDevice.ReadCoils(0, 2);
            Console.Write("ReadCoils: ");

            for (int i = 0; i < coils.Length; i++)
            {
                Console.Write("{0} ", coils[i]);
            }

            bool[] discreteInputs = tcpDevice.ReadInput(0, 15);
            Console.WriteLine();
            Console.Write("ReadInputs: ");

            for (int i = 0; i < discreteInputs.Length; i++)
            {
                Console.Write("{0} ", discreteInputs[i]);
            }

            short[] inputRegisters = tcpDevice.ReadInputRegisters(0, 10);

            short[] holdingRegisters = tcpDevice.ReadHoldingRegisters(0, 10);




            tcpDevice.Disconnect();
            Console.ReadKey();

        }
    }
}
