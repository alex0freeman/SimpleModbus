using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ModbusImp
{
    enum MbErrors
    {
        IllegalFunction = 1,
        IllegalDataAddress,
        IllegalDataValue,
        SlaveDeviceFailure,
        Acknowlegment,
        SlaveDeviceBusy,
        NegativeAcknowledge,
        MemoryParityError,
        GatewayPathUnavailable,
        GatewayTargetdeviceFailedToRespond
    };

    enum MbFunctions
    {
        ReadCoils = 1,
        ReadInputs,
        ReadHoldingRegisters,
        ReadInputRegister
    };

    public class ModbusDevice<T> where T : MBContext
    {
        private T cntx; // Modbus context
        public byte SlaveId { get; set; }
        public ModbusDevice(T cntx, byte slaveId)
        {
            this.cntx = cntx;
            cntx.Connect();
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


        private bool[] ParseDiscretes(byte[] res, int num)
        {
            bool[] result = new bool[num];
            for (int i = 0; i < num; i++)
            {
                int cur = (i >= 8) ? 0 : i;
                byte bit_mask = (byte)(1 << cur);
                result[i] = Convert.ToBoolean(res[(i/8)] & bit_mask);
                Console.WriteLine(result[i]);
             }
            
            return result;
        }

        public bool[] ReadCoils(ushort startAddress, ushort numItems)
        {
            byte[] res = Read((byte)MbFunctions.ReadCoils, startAddress, numItems);           
            return ParseDiscretes(res, numItems);
        }

        public bool[] ReadInput(ushort startAddress, ushort numItems)
        {
            byte[] res = Read((byte)MbFunctions.ReadInputs, startAddress, numItems);
            Console.WriteLine(BitConverter.ToString(res));
            return ParseDiscretes(res, numItems);
        }

        public void Disconnect()
        {
            cntx.Disconnect();
        }

    }
}
