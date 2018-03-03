using System;
using System.Linq;
using System.Runtime.InteropServices;


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
    struct MBWriteSingleCoil
    {
        public ushort address;
        public ushort writeValue;

        public MBWriteSingleCoil(ushort address, ushort writeValue)
        {
            this.address = BitConverter.ToUInt16((BitConverter.GetBytes(address)).Reverse().ToArray(), 0); ;
            this.writeValue = BitConverter.ToUInt16((BitConverter.GetBytes(writeValue)).Reverse().ToArray(), 0); ;
        }
    }



    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBWriteSingleRegister
    {
        public ushort address;
        public ushort writeValue;

        public MBWriteSingleRegister(ushort address, ushort writeValue)
        {
            this.address = BitConverter.ToUInt16((BitConverter.GetBytes(address)).Reverse().ToArray(), 0); 
            this.writeValue = BitConverter.ToUInt16((BitConverter.GetBytes(writeValue)).Reverse().ToArray(), 0); 
        }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBWriteMultiplyCoils
    {
        public ushort startAddress;
        public ushort countItems;
        public byte nextByteCount;
        public byte[] data;

        public MBWriteMultiplyCoils(ushort startAddress, ushort countItems, byte nextByteCount, byte[] data)
        {
            this.startAddress = BitConverter.ToUInt16((BitConverter.GetBytes(startAddress)).Reverse().ToArray(), 0); 
            this.countItems = BitConverter.ToUInt16((BitConverter.GetBytes(countItems)).Reverse().ToArray(), 0);
            this.nextByteCount = nextByteCount;
            this.data = data;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBWriteMultiplyHoldingRegisters
    {
        public ushort startAddress { get; set; }
        public ushort countItems { get; set; }
        public byte nextByteCount;
        public byte[] data;

        public MBWriteMultiplyHoldingRegisters(ushort startAddress, ushort countItems, byte nextByteCount, byte[] data)
        {
            this.startAddress = BitConverter.ToUInt16((BitConverter.GetBytes(startAddress)).Reverse().ToArray(), 0);
            this.countItems = BitConverter.ToUInt16((BitConverter.GetBytes(countItems)).Reverse().ToArray(), 0);
            this.nextByteCount = nextByteCount;
            this.data = data;
        }
    }
}
