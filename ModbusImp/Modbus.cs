using System;
using System.Linq;

namespace ModbusImp
{
    /// <summary>
    /// Represents context for single Modbus device
    /// </summary>
    /// <typeparam name="T">Connection type</typeparam>
    public class ModbusDevice<T> where T : IMBContext
    {
        /// <summary>
        /// Modbus device context
        /// </summary>
        private T cntx;
        /// <summary>
        /// Unique identificator of slave
        /// </summary>
        private byte SlaveId { get; set; }
        /// <summary>
        /// TODO: ???
        /// </summary>
        public int expectedResponseBytes;
        
        public ModbusDevice(T cntx, byte slaveId)
        {
            this.cntx = cntx;
            SlaveId = slaveId;
        }

        /// <summary>
        /// Returns information about active connection
        /// </summary>
        public string ConnectionCredentials()
        {
            return cntx.ConnectionCredentials();
        }

        /// <summary>
        /// Read data from current context
        /// </summary>
        /// <param name="funcNumber">Code of Modbus function</param>
        /// <param name="data">Data </param>
        /// <returns></returns>
        private byte[] Read(byte funcNumber, byte[] data)
        {
            byte[] message = cntx.BuildMessage(SlaveId, funcNumber, data);
            expectedResponseBytes += cntx.GetHeader();
            byte[] response = new byte[expectedResponseBytes];
            cntx.SendMsg(message);
            var cnt = cntx.RecieveMsg(ref response);
            // FIXME: Assert with count of readed bytes?
            return cntx.GetContent(response, expectedResponseBytes);
        }

        /// <summary>
        /// Write single reigster for current device
        /// </summary>
        /// <param name="functionCode">Modbus function code</param>
        /// <param name="data">Data to write</param>
        /// <returns>Operation status</returns>
        bool WriteRegister(byte functionCode, byte[] data)
        {
            byte[] message = cntx.BuildMessage(SlaveId, functionCode, data);
            expectedResponseBytes = message.Length;
            byte[] response = new byte[expectedResponseBytes];
            cntx.SendMsg(message);
            int cnt = cntx.RecieveMsg(ref response);
            return Enumerable.SequenceEqual(response, message);
        }
        
        /// <summary>
        /// Write multiple registers for current device
        /// </summary>
        /// <param name="functionCode">Modbus function code</param>
        /// <param name="data">Data to write</param>
        /// <returns>Number of writed bytes</returns>
        int WriteRegisters(byte functionCode, byte[] data)
        {
            byte[] content = Read(functionCode, data);
            return content.Last();
        }

        public bool[] ReadCoils(ushort startAddress, ushort itemCount)
        {
            MBReadCoils coilsData = new MBReadCoils(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadCoils>.GetExpectedBytesByFunction((int)MbFunctions.ReadCoils, itemCount);
            var coils = Read((byte)MbFunctions.ReadCoils, TypeManager<MBReadCoils>.ToBytes(coilsData));
            return TypeManager<MBReadCoils>.ParseDiscretes(coils, itemCount);
        }

        public bool[] ReadDiscreteInputs(ushort startAddress, ushort itemCount)
        {
            MBReadDiscretes discretesData = new MBReadDiscretes(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadDiscretes>.GetExpectedBytesByFunction((int)MbFunctions.ReadDiscreteInputs, itemCount);
            var discreteInputs = Read((byte)MbFunctions.ReadDiscreteInputs, TypeManager<MBReadDiscretes>.ToBytes(discretesData));
            return TypeManager<MBReadDiscretes>.ParseDiscretes(discreteInputs, itemCount);
        }

        public short[] ReadInputs(ushort startAddress, ushort itemCount)
        {
            MBReadInputRegisters intputRegisterData = new MBReadInputRegisters(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadInputRegisters>.GetExpectedBytesByFunction((int)MbFunctions.ReadInputs, itemCount);
            var inputRegisters = Read((byte)MbFunctions.ReadInputs, TypeManager<MBReadInputRegisters>.ToBytes(intputRegisterData));
            return TypeManager<MBReadInputRegisters>.ParseRegisters(inputRegisters, itemCount);
        }

        public short[] ReadHoldings(ushort startAddress, ushort itemCount)
        {
            MBReadHoldingRegisters hodingRegistersData = new MBReadHoldingRegisters(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadHoldingRegisters>.GetExpectedBytesByFunction((int)MbFunctions.ReadHoldings, itemCount);
            var holdingRegisters = Read((byte)MbFunctions.ReadHoldings, TypeManager<MBReadHoldingRegisters>.ToBytes(hodingRegistersData));
            return TypeManager<MBReadHoldingRegisters>.ParseRegisters(holdingRegisters, itemCount);
        }

        public bool WriteSingleCoil(ushort address, ushort value)
        {
            var writeCoilData = new MBWriteSingleCoil(address, value);
            var writeResult = WriteRegister((byte)MbFunctions.WriteSingleCoil, TypeManager<MBWriteSingleCoil>.ToBytes(writeCoilData));
            return writeResult;
        }

        public bool WriteSingleHolding(ushort address, ushort value)
        {
            var writeHoldingData = new MBWriteSingleHolding(address, value);
            var writeResult = WriteRegister((byte)MbFunctions.WriteSingleHolding, TypeManager<MBWriteSingleHolding>.ToBytes(writeHoldingData));
            return writeResult;
        }

        public int WriteCoils(ushort address, ushort countItems, byte nextByteCount, byte[] data)
        {
            var writeCoilsData = new MBWriteCoils(address, countItems, nextByteCount, data);
            expectedResponseBytes = TypeManager<MBWriteCoils>.GetExpectedBytesByFunction((int)MbFunctions.WriteCoils, countItems);
            var writeCountBytes = WriteRegisters((byte)MbFunctions.WriteCoils, TypeManager<MBWriteCoils>.ToBytes(writeCoilsData));
            return writeCountBytes;
        }

        public int WriteHoldings(ushort address, ushort countItems, byte nextByteCount, byte[] data)
        {
            var writeHoldingsData = new MBWriteHoldings(address, countItems, nextByteCount, data);
            expectedResponseBytes = TypeManager<MBWriteHoldings>.GetExpectedBytesByFunction((int)MbFunctions.WriteHoldings, countItems);
            var writeCountBytes = WriteRegisters((byte)MbFunctions.WriteHoldings, TypeManager<MBWriteHoldings>.ToBytes(writeHoldingsData));
            return writeCountBytes;
        }

        public void Connect()
        {
            cntx.Connect();
        }

        public void Disconnect()
        {
            cntx.Disconnect();
        }
    }
}
