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
            bool[] res = tcpDevice.ReadCoils(0, 2);
            Console.Write("ReadCoils: ");

            for (int i = 0; i < res.Length; i++)
            {
                Console.Write("{0} ", res[i]);
            }

            bool[] x = tcpDevice.ReadInput(0, 15);
            Console.WriteLine();
            Console.Write("ReadInputs: ");

            for (int i = 0; i < x.Length; i++)
            {
                Console.Write("{0} ", x[i]);
            }

            tcpDevice.Disconnect();
            Console.ReadKey();

        }
    }
}
