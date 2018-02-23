﻿using System;
using System.Runtime.InteropServices;
using System.Linq;

namespace ModbusImp
{
    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBRequestReadWriteOne 
    {
        byte slaveId { get; set; }
        byte functionId { get; set; }   
        ushort startAddress { get; set; }
        ushort readCnt { get; set; }

        public MBRequestReadWriteOne(byte slaveId, byte functionId, ushort startAddress, ushort readCnt)
        {
            this.slaveId = slaveId;
            this.functionId = functionId;
            this.startAddress = BitConverter.ToUInt16((BitConverter.GetBytes(startAddress)).Reverse().ToArray(), 0);
            this.readCnt = BitConverter.ToUInt16((BitConverter.GetBytes(readCnt)).Reverse().ToArray(), 0); 
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MBRequestWrite 
    {
        byte slaveId { get; set; }
        byte functionId { get; set; }
        ushort startAddress { get; set; }
        ushort readCnt { get; set; }
        byte byteSequenceCnt { get; set; }

        public MBRequestWrite(byte slaveId, byte functionId, ushort startAddress, ushort readCnt, byte byteSequenceCnt)
        {
            this.slaveId = slaveId;
            this.functionId = functionId;
            this.startAddress = BitConverter.ToUInt16((BitConverter.GetBytes(startAddress)).Reverse().ToArray(), 0);
            this.readCnt = BitConverter.ToUInt16((BitConverter.GetBytes(readCnt)).Reverse().ToArray(), 0);
            this.byteSequenceCnt = byteSequenceCnt;
        }
        
    }






}