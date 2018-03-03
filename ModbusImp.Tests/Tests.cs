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
        public void TestReadCoils()
        {
            var coils = SlaveDeviceContext.ReadCoils(0, 2);

            System.Diagnostics.Debug.WriteLine("Read coils:");
            foreach (var coil in coils)
            {
                System.Diagnostics.Debug.Write("{0} ", coil.ToString());
            }
            
            Assert.True(true);
        }
        
        [Test]
        public void TestReadDiscreteInputs()
        {
            var discreteInputs = SlaveDeviceContext.ReadInput(0, 15);

            System.Diagnostics.Debug.WriteLine("Read discrete inputs:");
            foreach (var input in discreteInputs)
            {
                System.Diagnostics.Debug.Write("{0} ", input.ToString());
            }
            
            Assert.True(true);
        }
        
        [Test]
        public void TestReadInputs()
        {
            var inputs = SlaveDeviceContext.ReadInputRegisters(0, 10);

            System.Diagnostics.Debug.WriteLine("Read input registers:");
            foreach (var input in inputs)
            {
                System.Diagnostics.Debug.Write("{0} ", input.ToString());
            }
            
            Assert.True(true);
        }
        
        [Test]
        public void TestReadHoldings()
        {
            var holdings = SlaveDeviceContext.ReadHoldingRegisters(0, 10);

            System.Diagnostics.Debug.WriteLine("Read input registers:");
            foreach (var holding in holdings)
            {
                System.Diagnostics.Debug.Write("{0} ", holding.ToString());
            }
            
            Assert.True(true);
        }
    }
}