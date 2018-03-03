using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
            byte[] coils = Read((byte)MbFunctions.ReadCoils, TypeManager<MBReadCoils>.ToBytes(coilsData));
            return TypeManager<MBReadCoils>.ParseDiscretes(coils, itemCount);
        }

        public bool[] ReadInput(ushort startAddress, ushort itemCount)
        {
            MBReadDiscretes discretesData = new MBReadDiscretes(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadDiscretes>.GetExpectedBytesByFunction((int)MbFunctions.ReadInputs, itemCount);
            byte[] discreteInputs = Read((byte)MbFunctions.ReadInputs, TypeManager<MBReadDiscretes>.ToBytes(discretesData));
            return TypeManager<MBReadDiscretes>.ParseDiscretes(discreteInputs, itemCount);
        }

        public short[] ReadInputRegisters(ushort startAddress, ushort itemCount)
        {
            MBReadInputRegisters intputRegisterData = new MBReadInputRegisters(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadInputRegisters>.GetExpectedBytesByFunction((int)MbFunctions.ReadInputRegister, itemCount);
            byte[] inputRegisters = Read((byte)MbFunctions.ReadInputRegister, TypeManager<MBReadInputRegisters>.ToBytes(intputRegisterData));
            return TypeManager<MBReadInputRegisters>.ParseRegisters(inputRegisters, itemCount);
        }

        public short[] ReadHoldingRegisters(ushort startAddress, ushort itemCount)
        {
            MBReadHoldingRegisters hodingRegistersData = new MBReadHoldingRegisters(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadHoldingRegisters>.GetExpectedBytesByFunction((int)MbFunctions.ReadHoldingRegisters, itemCount);
            byte[] holdingRegisters = Read((byte)MbFunctions.ReadHoldingRegisters, TypeManager<MBReadHoldingRegisters>.ToBytes(hodingRegistersData));
            return TypeManager<MBReadHoldingRegisters>.ParseRegisters(holdingRegisters, itemCount);
        }

        public bool WriteSingleCoil(ushort address, ushort value)
        {
            MBWriteSingleCoil writeCoilData = new MBWriteSingleCoil(address, value);
            bool writeResult = WriteSingle((byte)MbFunctions.WriteSingleCoil, TypeManager<MBWriteSingleCoil>.ToBytes(writeCoilData));
            return writeResult;
        }

        public bool WriteSingleRegister(ushort address, ushort value)
        {
            MBWriteSingleCoil writeCoilData = new MBWriteSingleCoil(address, value);
            bool writeResult = WriteSingle((byte)MbFunctions.WriteSingleCoil, TypeManager<MBWriteSingleCoil>.ToBytes(writeCoilData));
            return writeResult;
        }

        public int WriteMultiplyCoils(ushort address, ushort countItems, byte nextByteCount, byte[] data)
        {
            MBWriteMultiplyCoils writeCoilsData = new MBWriteMultiplyCoils(address, countItems, nextByteCount, data);
            expectedResponseBytes = TypeManager<MBWriteMultiplyCoils>.GetExpectedBytesByFunction((int)MbFunctions.WriteMultiplyCoils, countItems);
            int writeCountBytes = WriteMultiply((byte)MbFunctions.WriteMultiplyCoils, TypeManager<MBWriteMultiplyCoils>.ToBytes(writeCoilsData));
            return writeCountBytes;
        }

        public int WriteMultiplyRegisters(ushort address, ushort countItems, byte nextByteCount, byte[] data)
        {
            MBWriteMultiplyHoldingRegisters writeRegistersData = new MBWriteMultiplyHoldingRegisters(address, countItems, nextByteCount, data);
            expectedResponseBytes = TypeManager<MBWriteMultiplyHoldingRegisters>.GetExpectedBytesByFunction((int)MbFunctions.WriteMultiplyHoldingRegisters, countItems);
            int writeCountBytes = WriteMultiply((byte)MbFunctions.WriteMultiplyHoldingRegisters, TypeManager<MBWriteMultiplyHoldingRegisters>.ToBytes(writeRegistersData));
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
