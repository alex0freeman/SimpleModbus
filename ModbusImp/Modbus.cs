using System;
using System.Linq;
using System.Runtime.InteropServices;

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
        /// Server data exchange function
        /// </summary>
        /// <param name="funcNumber"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte[] Excange(byte funcNumber, byte[] data)
        {
            byte[] message = cntx.BuildMessage(SlaveId, funcNumber, data);
            expectedResponseBytes += cntx.GetHeader();
            byte[] response = new byte[expectedResponseBytes];
            cntx.SendMsg(message);
            var cnt = cntx.RecieveMsg(ref response);
            return response;
        }

        /// <summary>
        /// Read data from current context
        /// </summary>
        /// <param name="funcNumber">Code of Modbus function</param>
        /// <param name="data">Data </param>
        /// <returns></returns>
        /// 
        private byte[] Read(byte funcNumber, byte[] data)
        {
            byte[] response = Excange(funcNumber, data);
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
            byte[] response = Excange(functionCode, data);
            return response.Last();
        }

        /// <summary>
        /// Read coils from remote device
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="itemCount"></param>
        /// <returns>Array of coil values(True/False)</returns>
        public byte[] ReadCoils(ushort startAddress, ushort itemCount)
        {
            MBReadCoils coilsData = new MBReadCoils(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadCoils>.GetExpectedBytesByFunction((int)MbFunctions.ReadCoils, itemCount);
            var coils = Read((byte)MbFunctions.ReadCoils, TypeManager<MBReadCoils>.ToBytes(coilsData));
            return TypeManager<MBReadCoils>.ParseDiscretes(coils, itemCount);
        }


        /// <summary>
        /// Read discrete inputs from remote device
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="itemCount"></param>
        /// <returns>Array of discrete input values(True/False)</returns>
        public byte[] ReadDiscreteInputs(ushort startAddress, ushort itemCount)
        {
            MBReadDiscretes discretesData = new MBReadDiscretes(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadDiscretes>.GetExpectedBytesByFunction((int)MbFunctions.ReadDiscreteInputs, itemCount);
            var discreteInputs = Read((byte)MbFunctions.ReadDiscreteInputs, TypeManager<MBReadDiscretes>.ToBytes(discretesData));
            return TypeManager<MBReadDiscretes>.ParseDiscretes(discreteInputs, itemCount);
        }

        /// <summary>
        /// Read input registers from remote device
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="itemCount"></param>
        /// <returns>Array of input register values</returns>
        public short[] ReadInputs(ushort startAddress, ushort itemCount)
        {
            MBReadInputRegisters intputRegisterData = new MBReadInputRegisters(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadInputRegisters>.GetExpectedBytesByFunction((int)MbFunctions.ReadInputs, itemCount);
            var inputRegisters = Read((byte)MbFunctions.ReadInputs, TypeManager<MBReadInputRegisters>.ToBytes(intputRegisterData));
            return TypeManager<MBReadInputRegisters>.ParseRegisters(inputRegisters, itemCount);
        }


        /// <summary>
        /// Read holding registers from remote device
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="itemCount"></param>
        /// <returns>Array of holding register values</returns>
        public short[] ReadHoldings(ushort startAddress, ushort itemCount)
        {
            MBReadHoldingRegisters hodingRegistersData = new MBReadHoldingRegisters(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadHoldingRegisters>.GetExpectedBytesByFunction((int)MbFunctions.ReadHoldings, itemCount);
            var holdingRegisters = Read((byte)MbFunctions.ReadHoldings, TypeManager<MBReadHoldingRegisters>.ToBytes(hodingRegistersData));
            return TypeManager<MBReadHoldingRegisters>.ParseRegisters(holdingRegisters, itemCount);
        }

        /// <summary>
        /// Force/write single coil to remote device
        /// </summary>
        /// <param name="address">Address of coil</param>
        /// <param name="value">Value to force/write (on: 0xFF00, off: 0) </param>
        /// <returns>Recording result (true/ false)</returns>
        public bool WriteSingleCoil(ushort address, ushort value)
        {
            var writeCoilData = new MBWriteSingleCoil(address, value);
            var writeResult = WriteRegister((byte)MbFunctions.WriteSingleCoil, TypeManager<MBWriteSingleCoil>.ToBytes(writeCoilData));
            return writeResult;
        }


        /// <summary>
        /// Preset/write single holding register to remote device
        /// </summary>
        /// <param name="address">Address of holding register to preset/write</param>
        /// <param name="value"></param>
        /// <returns>Recording result (true/ false)</returns>
        public bool WriteSingleHolding(ushort address, ushort value)
        {
            var writeHoldingData = new MBWriteSingleHolding(address, value);
            var writeResult = WriteRegister((byte)MbFunctions.WriteSingleHolding, TypeManager<MBWriteSingleHolding>.ToBytes(writeHoldingData));
            return writeResult;
        }


        /// <summary>
        /// Force/write multiple coils to remote device
        /// </summary>
        /// <param name="address">Address of first coil to force/write</param>
        /// <param name="countItems">Number of coils to force/write</param>
        /// <param name="nextByteCount">Number of bytes of coil values to follow</param>
        /// <param name="data">Coil values (8 coil values per byte)</param>
        /// <returns>Number of recorded coils</returns>
        public int WriteCoils(ushort address, ushort countItems, byte nextByteCount, byte[] data)
        {
            var writeCoilsData = new MBWriteCoils(address, countItems, nextByteCount);
            var allNumbers = TypeManager<MBWriteCoils>.ToBytes(writeCoilsData).Concat(data).ToArray();
            expectedResponseBytes = TypeManager<MBWriteCoils>.GetExpectedBytesByFunction((int)MbFunctions.WriteCoils, countItems);
            var writeCountBytes = WriteRegisters((byte)MbFunctions.WriteCoils, allNumbers);
            return writeCountBytes;
        }

        /// <summary>
        /// Preset/write multiple holding registers
        /// </summary>
        /// <param name="address">Address of first holding register to preset/write</param>
        /// <param name="countItems">Number of holding registers to preset/write</param>
        /// <param name="nextByteCount">Number of bytes of register values to follow</param>
        /// <param name="data">New values of holding registers (16 bits per register)</param>
        /// <returns>Number of preset/written holding registers</returns>
        public int WriteHoldings(ushort address, ushort countItems, byte nextByteCount, short[] data)
        {
            var writeHoldingsData = new MBWriteHoldings(address, countItems, nextByteCount);
            var allNumbers = TypeManager<MBWriteHoldings>.ToBytes(writeHoldingsData).Concat(TypeManager<MBWriteHoldings>.ToBytes(data)).ToArray();
            expectedResponseBytes = TypeManager<MBWriteHoldings>.GetExpectedBytesByFunction((int)MbFunctions.WriteHoldings, countItems);
            var writeCountBytes = WriteRegisters((byte)MbFunctions.WriteHoldings, allNumbers);
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
