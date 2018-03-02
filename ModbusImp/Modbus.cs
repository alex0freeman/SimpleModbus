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
            byte[] buff = new byte[256]; // Buffer to save response
            byte[] message = cntx.BuildMessage(SlaveId, funcNumber, data);
            cntx.SendMsg(message);
            int cnt = cntx.RecieveMsg(ref buff);
            byte[] responce = new byte[cnt];
            Array.Copy(buff, responce, cnt);
            return cntx.GetContent(responce, expectedResponseBytes); ;
        }


        private bool[] ParseDiscretes(byte[] responseData, int count)
        {
            bool[] discreteArray = new bool[count];
            for (int i = 0; i < count; i++)
            {
                int cur = (i >= 8) ? 0 : i;
                byte bitMask = (byte)(1 << cur);
                discreteArray[i] = Convert.ToBoolean(responseData[(i/8)] & bitMask);
            }

            return discreteArray;
        }

        private short ReverseBytes(short value)
        {
            return (short)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        private short[] ParseRegisters(byte[] responseData, int count)
        {
            short[] registersArray = new short[count];

            Buffer.BlockCopy(responseData, 0, registersArray, 0, responseData.Length);
            return registersArray.Select(x => ReverseBytes(x)).ToArray();
        }

        public bool[] ReadCoils(ushort startAddress, ushort itemCount)
        {
            MBReadCoils coilsData = new MBReadCoils(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadCoils>.GetExpectedBytesByFunction((int)MbFunctions.ReadCoils, itemCount);
            byte[] coils = Read((byte)MbFunctions.ReadCoils, TypeManager<MBReadCoils>.ToBytes(coilsData));
            return ParseDiscretes(coils, itemCount);
        }

        public bool[] ReadInput(ushort startAddress, ushort itemCount)
        {
            MBReadDiscretes discretesData = new MBReadDiscretes(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadDiscretes>.GetExpectedBytesByFunction((int)MbFunctions.ReadInputs, itemCount);
            byte[] discreteInputs = Read((byte)MbFunctions.ReadInputs, TypeManager<MBReadDiscretes>.ToBytes(discretesData));
            Console.WriteLine(BitConverter.ToString(discreteInputs));
            return ParseDiscretes(discreteInputs, itemCount);
        }

        public short[] ReadInputRegisters(ushort startAddress, ushort itemCount)
        {
            MBReadInputRegisters intputRegisterData = new MBReadInputRegisters(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadInputRegisters>.GetExpectedBytesByFunction((int)MbFunctions.ReadInputRegister, itemCount);
            byte[] inputRegisters = Read((byte)MbFunctions.ReadInputRegister, TypeManager<MBReadInputRegisters>.ToBytes(intputRegisterData));
            return ParseRegisters(inputRegisters, itemCount);
        }

        public short[] ReadHoldingRegisters(ushort startAddress, ushort itemCount)
        {
            MBReadHoldingRegisters hodingRegistersData = new MBReadHoldingRegisters(startAddress, itemCount);
            expectedResponseBytes = TypeManager<MBReadHoldingRegisters>.GetExpectedBytesByFunction((int)MbFunctions.ReadHoldingRegisters, itemCount);
            byte[] holdingRegisters = Read((byte)MbFunctions.ReadHoldingRegisters, TypeManager<MBReadHoldingRegisters>.ToBytes(hodingRegistersData));
            return ParseRegisters(holdingRegisters, itemCount);
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
