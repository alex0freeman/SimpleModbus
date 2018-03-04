using System;
using System.Net;
using NUnit.Framework;

namespace ModbusImp.Tests
{
    /// <summary>
    /// Currently we have no Modbus Server implementation, so I use alongside Modbus TCP Slave on localhost:5002.
    /// It can be represented as following Python script (via pyModbusTCP module):
    /// 
    /// from pyModbusTCP.server import ModbusServer
    /// server = ModbusServer(host='localhost', port=5002)
    /// server.start()
    /// 
    /// I should fix it later when Modbus Server will be implemented.
    /// </summary>
    [TestFixture]
    public class ModusImpTests
    {
        /// <summary>
        /// Slave credentials
        /// </summary>
        private const string Hostname = "127.0.0.1";
        private const int Port = 5002;
        private const int SlaveId = 1;

        private ModbusDevice<TCPContext> SlaveDeviceContext;

        /// <summary>
        /// Initialize connection context
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            SlaveDeviceContext = new ModbusDevice<TCPContext>(new TCPContext(Hostname, Port), SlaveId);
            SlaveDeviceContext.Connect();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            SlaveDeviceContext.Disconnect();
        }

        [Test]
        [Category("Read")]
        public void TestRandomReadCoils()
        {
            const int registersToRead = 2;
            var coils = SlaveDeviceContext.ReadCoils(0, registersToRead);

            Console.WriteLine("Read coils:");
            foreach (var coil in coils)
            {
                Console.Write("{0} ", coil.ToString());
            }
            
            Assert.AreEqual(coils.Length, registersToRead);
        }
        
        [Test]
        [Category("Read")]
        public void TestRandomReadDiscreteInputs()
        {
            const int registersToRead = 15;
            var discreteInputs = SlaveDeviceContext.ReadDiscreteInputs(0, registersToRead);

            Console.WriteLine("Read discrete inputs:");
            foreach (var input in discreteInputs)
            {
                Console.Write("{0} ", input.ToString());
            }
            
            Assert.AreEqual(discreteInputs.Length, registersToRead);
        }
        
        [Test]
        [Category("Read")]
        public void TestRandomReadInputs()
        {
            const int registersToRead = 10;
            var inputs = SlaveDeviceContext.ReadInputs(0, registersToRead);

            Console.WriteLine("Read input registers:");
            foreach (var input in inputs)
            {
                Console.Write("{0} ", input.ToString());
            }
            
            Assert.AreEqual(inputs.Length, registersToRead);
        }
        
        [Test]
        [Category("Read")]
        public void TestRandomReadHoldings()
        {
            const int registersToRead = 10;
            var holdings = SlaveDeviceContext.ReadHoldings(0, registersToRead);

            Console.WriteLine("Read input registers:");
            foreach (var holding in holdings)
            {
                Console.Write("{0} ", holding.ToString());
            }
            
            Assert.AreEqual(holdings.Length, registersToRead);
        }
        
        [Test]
        [Category("Write")]
        public void TestWriteSingleCoil()
        {
            const ushort value = 19;
            var result = SlaveDeviceContext.WriteSingleCoil(0, value);

            Console.WriteLine("Write single coil: {0}", result);
            
            Assert.AreEqual(result, true);
        }
        
        [Test]
        [Category("Write")]
        public void TestWriteSingleHolding()
        {
            const ushort value = 19;
            var result = SlaveDeviceContext.WriteSingleHolding(0, value);

            Console.WriteLine("Write single holding: {0}", result);
            
            Assert.AreEqual(result, true);
        }
        
        [Test]
        [Category("Write")]
        public void TestWriteCoils()
        {
            var data = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            var result = SlaveDeviceContext.WriteCoils(1, (ushort)(data.Length-1), 2, data);

            Console.WriteLine("Write coils: {0} bytes was written", result);
            
            Assert.AreEqual(result, data.Length);
        }
    }
}