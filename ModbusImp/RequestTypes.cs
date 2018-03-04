using System;
using System.Linq;
using System.Runtime.InteropServices;

// TODO: Reverse Bytes in TypeManager
namespace ModbusImp
{
    /// <summary>
    /// Read Coil Registers
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBReadCoils
    {
        public ushort startAddress;
        public ushort readCnt;
        public MBReadCoils(ushort startAddress, ushort readCnt)
        {
            this.startAddress = BitConverter.ToUInt16(BitConverter.GetBytes(startAddress).Reverse().ToArray(), 0);
            this.readCnt = BitConverter.ToUInt16(BitConverter.GetBytes(readCnt).Reverse().ToArray(), 0);
        }
    }

    /// <summary>
    /// Read Discrete Input Registers
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBReadDiscretes
    {
        public ushort startAddress;
        public ushort readCnt;

        public MBReadDiscretes(ushort startAddress, ushort readCnt)
        {
            this.startAddress = BitConverter.ToUInt16(BitConverter.GetBytes(startAddress).Reverse().ToArray(), 0);
            this.readCnt = BitConverter.ToUInt16(BitConverter.GetBytes(readCnt).Reverse().ToArray(), 0);
        }
    }

    /// <summary>
    /// Read Input Registers
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBReadInputRegisters
    {
        public ushort startAddress;
        public ushort readCnt;

        public MBReadInputRegisters(ushort startAddress, ushort readCnt)
        {
            this.startAddress = BitConverter.ToUInt16(BitConverter.GetBytes(startAddress).Reverse().ToArray(), 0);
            this.readCnt = BitConverter.ToUInt16(BitConverter.GetBytes(readCnt).Reverse().ToArray(), 0);
        }
    }

    /// <summary>
    /// Read Holding Registers
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBReadHoldingRegisters
    {
        public ushort startAddress;
        public ushort readCnt;

        public MBReadHoldingRegisters(ushort startAddress, ushort readCnt)
        {
            this.startAddress = BitConverter.ToUInt16(BitConverter.GetBytes(startAddress).Reverse().ToArray(), 0); ;
            this.readCnt = BitConverter.ToUInt16(BitConverter.GetBytes(readCnt).Reverse().ToArray(), 0);
        }
    }

    /// <summary>
    /// Write single Coil Register
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBWriteSingleCoil
    {
        public ushort address;
        public ushort writeValue;

        public MBWriteSingleCoil(ushort address, ushort writeValue)
        {
            this.address = BitConverter.ToUInt16(BitConverter.GetBytes(address).Reverse().ToArray(), 0); ;
            this.writeValue = BitConverter.ToUInt16(BitConverter.GetBytes(writeValue).Reverse().ToArray(), 0); ;
        }
    }

    /// <summary>
    /// Write single Holding Register
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBWriteSingleHolding
    {
        public ushort address;
        public ushort writeValue;

        public MBWriteSingleHolding(ushort address, ushort writeValue)
        {
            this.address = BitConverter.ToUInt16(BitConverter.GetBytes(address).Reverse().ToArray(), 0); 
            this.writeValue = BitConverter.ToUInt16(BitConverter.GetBytes(writeValue).Reverse().ToArray(), 0); 
        }
    }

    /// <summary>
    /// Write multiple Coil Registers
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBWriteCoils
    {
        public ushort startAddress;
        public ushort countItems;
        public byte nextByteCount;

        /// <param name="startAddress">First register address to write</param>
        /// <param name="countItems">Quantity of registers to write</param>
        /// <param name="nextByteCount"></param>
        public MBWriteCoils(ushort startAddress, ushort countItems, byte nextByteCount)
        {
            this.startAddress = BitConverter.ToUInt16(BitConverter.GetBytes(startAddress).Reverse().ToArray(), 0); 
            this.countItems = BitConverter.ToUInt16(BitConverter.GetBytes(countItems).Reverse().ToArray(), 0);
            this.nextByteCount = nextByteCount;
        }
    }

    /// <summary>
    /// Write multiple Holding registers
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBWriteHoldings
    {
        public ushort startAddress { get; set; }
        public ushort countItems { get; set; }
        public byte nextByteCount;

        public MBWriteHoldings(ushort startAddress, ushort countItems, byte nextByteCount)
        {
            this.startAddress = BitConverter.ToUInt16(BitConverter.GetBytes(startAddress).Reverse().ToArray(), 0);
            this.countItems = BitConverter.ToUInt16(BitConverter.GetBytes(countItems).Reverse().ToArray(), 0);
            this.nextByteCount = nextByteCount;
        }
    }
}
