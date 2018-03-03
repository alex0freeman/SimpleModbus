using System;
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

        private ModbusDevice<MBContext> SlaveDeviceContext;

        /// <summary>
        /// Initialize connection context
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            Transport<TCPContex>.Register(1, () => new TCPContex(Hostname, Port));
            var tcp = Transport<TCPContex>.Create(1);
            SlaveDeviceContext = new ModbusDevice<MBContext>(tcp, 1);
            
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
        public void TestReadInputs()
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
        public void TestReadHoldings()
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
    }
}