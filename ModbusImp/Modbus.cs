using System.Linq;

namespace ModbusImp
{
    public class ModbusDevice<T> where T : MBContext
    {
        private T cntx; // Modbus context
        public byte SlaveId { get; set; }
        public int expectedResponseBytes;
        public ModbusDevice(T cntx, byte slaveId)
        {
            this.cntx = cntx;
            SlaveId = slaveId;
        }

        // Read data from current context
        private byte[] Read(byte funcNumber, byte[] data)
        {
            byte[] message = cntx.BuildMessage(SlaveId, funcNumber, data);
            expectedResponseBytes += cntx.GetHeader();
            byte[] response = new byte[expectedResponseBytes];
            cntx.SendMsg(message);
            int cnt = cntx.RecieveMsg(ref response);
            return cntx.GetContent(response, expectedResponseBytes);
        }

        bool WriteSingle(byte functionCode, byte[] data)
        {
            byte[] message = cntx.BuildMessage(SlaveId, functionCode, data);
            expectedResponseBytes = message.Length;
            byte[] response = new byte[expectedResponseBytes];
            cntx.SendMsg(message);
            int cnt = cntx.RecieveMsg(ref response);
            return Enumerable.SequenceEqual(response, message);
        }
        
        int WriteMultiply(byte functionCode, byte[] data)
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
            var writeResult = WriteSingle((byte)MbFunctions.WriteSingleCoil, TypeManager<MBWriteSingleCoil>.ToBytes(writeCoilData));
            return writeResult;
        }

        public bool WriteSingleHolding(ushort address, ushort value)
        {
            var writeHoldingData = new MBWriteSingleHolding(address, value);
            var writeResult = WriteSingle((byte)MbFunctions.WriteSingleHolding, TypeManager<MBWriteSingleHolding>.ToBytes(writeHoldingData));
            return writeResult;
        }

        public int WriteCoils(ushort address, ushort countItems, byte nextByteCount, byte[] data)
        {
            var writeCoilsData = new MBWriteMultiplyCoils(address, countItems, nextByteCount, data);
            expectedResponseBytes = TypeManager<MBWriteMultiplyCoils>.GetExpectedBytesByFunction((int)MbFunctions.WriteCoils, countItems);
            var writeCountBytes = WriteMultiply((byte)MbFunctions.WriteCoils, TypeManager<MBWriteMultiplyCoils>.ToBytes(writeCoilsData));
            return writeCountBytes;
        }

        public int WriteHoldings(ushort address, ushort countItems, byte nextByteCount, byte[] data)
        {
            var writeHoldingsData = new MBWriteMultiplyHoldingRegisters(address, countItems, nextByteCount, data);
            expectedResponseBytes = TypeManager<MBWriteMultiplyHoldingRegisters>.GetExpectedBytesByFunction((int)MbFunctions.WriteHoldings, countItems);
            var writeCountBytes = WriteMultiply((byte)MbFunctions.WriteHoldings, TypeManager<MBWriteMultiplyHoldingRegisters>.ToBytes(writeHoldingsData));
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
