// gRPC server that accepts requests on Modbus functions

using System;
using System.Threading.Tasks;
using Grpc.Core;
using ModbusImp;

namespace ModbusImp.Service
{
    class ModbusImpl : Modbus.ModbusBase
    {
        // Server side handler of the SayHello RPC
        public override Task<TCPResponse> GetTCP(TCPRequest request, ServerCallContext context)
        {
            ModbusImp.TCPRequest ttcp = new ModbusImp.TCPRequest(1, 3, 3, 2);
            return Task.FromResult(new TCPResponse { Seq = "Hello" });
        }
    }

    class Program
    {
        const int Port = 50051;

        public static void Main(string[] args)
        {
            Server server = new Server
            {
                Services = { Modbus.BindService(new ModbusImpl()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Modbus gRPC server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
