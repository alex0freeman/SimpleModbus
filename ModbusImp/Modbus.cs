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
        public ModbusDevice(T cntx, byte slaveId)
        {
            this.cntx = cntx;
            SlaveId = slaveId;
        }

        // Read data from current context
        private byte[] Read(byte funcNumber, ushort startAddress, ushort numItems)
        {
            byte[] buff = new byte[256]; // Buffer to save response
            byte[] message = cntx.BuildMessage(SlaveId, funcNumber, startAddress, numItems);
            cntx.SendMsg(message);
            int cnt = cntx.RecieveMsg(ref buff);
            byte[] responce = new byte[cnt];
            Array.Copy(buff, responce, cnt);
            return cntx.GetContent(responce); ;
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
            byte[] coils = Read((byte)MbFunctions.ReadCoils, startAddress, itemCount);           
            return ParseDiscretes(coils, itemCount);
        }

        public bool[] ReadInput(ushort startAddress, ushort itemCount)
        {
            byte[] discreteInputs = Read((byte)MbFunctions.ReadInputs, startAddress, itemCount);
            Console.WriteLine(BitConverter.ToString(discreteInputs));
            return ParseDiscretes(discreteInputs, itemCount);
        }

        public short[] ReadInputRegisters(ushort startAddress, ushort itemCount)
        {
            byte[] inputRegisters = Read((byte)MbFunctions.ReadInputRegister, startAddress, itemCount);
            return ParseRegisters(inputRegisters, itemCount);
        }

        public short[] ReadHoldingRegisters(ushort startAddress, ushort itemCount)
        {
            byte[] holdingRegisters = Read((byte)MbFunctions.ReadHoldingRegisters, startAddress, itemCount);
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
