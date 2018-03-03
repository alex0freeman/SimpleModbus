using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Utils;
using NUnit.Framework;

namespace ModbusImp.Service.Test
{
    [TestFixture]
    public class Tests
    {
        private const string ServerHost = "localhost";
        private const int ServerPort = 50051;
        Server server;
        Channel channel;
        ModbusImp.S client;
        
        [OneTimeSetUp]
        public void Init()
        {
            server = new Server(new[] {new ChannelOption(ChannelOptions.SoReuseport, 0)})
            {
                Services = { ModbusTCP.BindService(new ModbusImp()) },
                Ports = { new ServerPort("0.0.0.0", Port, ServerCredentials.Insecure) }
            };
            server.Start();
            {
                Services = { Math.BindService(new MathServiceImpl()) },
                Ports = { { Host, ServerPort.PickUnused, ServerCredentials.Insecure } }
            };
            server.Start();
            channel = new Channel(Host, server.Ports.Single().BoundPort, ChannelCredentials.Insecure);
            client = new Math.MathClient(channel);
        }
        
        [Test]
        public void Test1()
        {
            Assert.True(true);
        }
    }
}