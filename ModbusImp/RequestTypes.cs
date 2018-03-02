using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


//TODO Reverse Bytes in TypeManager
namespace ModbusImp
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBReadCoils
    {
        public ushort startAddress;
        public ushort readCnt;
        public MBReadCoils(ushort startAddress, ushort readCnt)
        {
            this.startAddress = BitConverter.ToUInt16((BitConverter.GetBytes(startAddress)).Reverse().ToArray(), 0); ;
            this.readCnt = BitConverter.ToUInt16((BitConverter.GetBytes(readCnt)).Reverse().ToArray(), 0); ;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBReadDiscretes
    {
        public ushort startAddress;
        public ushort readCnt;

        public MBReadDiscretes(ushort startAddress, ushort readCnt)
        {
            this.startAddress = BitConverter.ToUInt16((BitConverter.GetBytes(startAddress)).Reverse().ToArray(), 0); ;
            this.readCnt = BitConverter.ToUInt16((BitConverter.GetBytes(readCnt)).Reverse().ToArray(), 0); ;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBReadInputRegisters
    {
        public ushort startAddress;
        public ushort readCnt;

        public MBReadInputRegisters(ushort startAddress, ushort readCnt)
        {
            this.startAddress = BitConverter.ToUInt16((BitConverter.GetBytes(startAddress)).Reverse().ToArray(), 0); ;
            this.readCnt = BitConverter.ToUInt16((BitConverter.GetBytes(readCnt)).Reverse().ToArray(), 0); ;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBReadHoldingRegisters
    {
        public ushort startAddress;
        public ushort readCnt;

        public MBReadHoldingRegisters(ushort startAddress, ushort readCnt)
        {
            this.startAddress = BitConverter.ToUInt16((BitConverter.GetBytes(startAddress)).Reverse().ToArray(), 0); ;
            this.readCnt = BitConverter.ToUInt16((BitConverter.GetBytes(readCnt)).Reverse().ToArray(), 0); ;
        }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct WriteSingleCoil
    {
        public ushort startAddress;
        public ushort writeValue;
    }



    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct WriteSingleRegister
    {
        public ushort startAddress;
        public ushort writeValue;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct WriteMultiplyCoils
    {
        public ushort startAddress;
        public ushort countItems;
        public byte nextByteCount;
        public byte[] data;

    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct WriteMultiplyHoldingRegisters
    {
        public ushort startAddress { get; set; }
        public ushort countItems { get; set; }
        public byte nextByteCount;
        public byte[] data;
    }





}
