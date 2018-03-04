using System;
using Grpc.Core;
using Grpc.Core.Utils;
using NUnit.Framework;

namespace ModbusImp.Service.Tests
{
    [TestFixture]
    public class Tests
    {
        private const string Hostname = "0.0.0.0";
        private const int Port = 50049;
        Server server;
        Channel channel;
        ModbusTCP.ModbusTCPClient client;

        [OneTimeSetUp]
        public void Init()
        {
            // Disable SO_REUSEPORT to prevent https://github.com/grpc/grpc/issues/10755
            server = new Server(new[] { new ChannelOption(ChannelOptions.SoReuseport, 0) })
            {
                Services = { ModbusTCP.BindService(new ModbusImpl()) },
                Ports = { { Hostname, Port, ServerCredentials.Insecure } }
            };
            server.Start();
            channel = new Channel(Hostname, Port, ChannelCredentials.Insecure);
            client = new ModbusTCP.ModbusTCPClient(channel);
        }
        
        [OneTimeTearDown]
        public void Cleanup()
        {
            channel.ShutdownAsync().Wait();
            server.ShutdownAsync().Wait();
        }
        
        [Test]
        public void TestConnectDevice()
        {
            var response = client.ConnectDevice(new ModbusConnectionRequest
            {
                Ip = "localhost",
                Port = 5002
            });
            Assert.AreEqual(true, response);
        }
    }
}