// gRPC server that accepts requests on Modbus functions

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;

namespace ModbusImp.Service
{
    /// <summary>
    /// Implementation of gRPC server that performs access to Modbus devices from any gRPC client
    /// </summary>
    public class ModbusImpl : ModbusTCP.ModbusTCPBase
    {
        /// <summary>
        /// Pool that contains available Modbus devices contexts
        /// </summary>
        private ModbusContextMultiton DeviceContextsPool;

        /// <summary>
        /// Client new Modbus connection initialize
        /// </summary>
        public override Task<ModbusConnectionResponse> ConnectDevice(ModbusConnectionRequest request,
            ServerCallContext context)
        {
            var address = request.Ip;
            var port = (ushort) request.Port;
            var device = new Tuple<string, ushort>(address, port);
            
            // Register device context
            DeviceContextsPool = ModbusContextMultiton.GetInstance(device);
            
            // Initialize connection
            DeviceContextsPool.ModbusContext.Connect();
            
            // Send connection status
            return Task.FromResult(new ModbusConnectionResponse { Connected = true });
        }
        
        /// <summary>
        /// Reading request to Modbus target device
        /// </summary>
        public override Task<ModbusReadResponse> ReadModbus(ModbusReadRequest request, ServerCallContext context)
        {
            // Get required device
            var address = Convert.ToString(request.Ip);
            var port = Convert.ToUInt16(request.Port);
            var device = new Tuple<string, ushort>(address, port);
            DeviceContextsPool = ModbusContextMultiton.GetInstance(device);
            
            var registerType = request.RegisterType;
            var startAddress = Convert.ToUInt16(request.StartAddress);
            var readCnt = Convert.ToUInt16(request.ReadCnt);
            
            // Request Modbus slave 
            // TODO: Fix type conversions in library
            // TODO: Think more about logic to call required function
            byte[] seq = null;
            switch (registerType)
            {
                case ModbusRegisters.Coil:
                    var resultCoil = DeviceContextsPool.ModbusContext.ReadCoils(startAddress, readCnt);
                    seq = Array.ConvertAll(resultCoil, b => b ? (byte) 1 : (byte) 0);
                    break;
                case ModbusRegisters.DiscreteInput:
                    var resultInput = DeviceContextsPool.ModbusContext.ReadDiscreteInputs(startAddress, readCnt);
                    seq = Array.ConvertAll(resultInput, b => b ? (byte) 1 : (byte) 0);
                    break;
                case ModbusRegisters.Holding:
//                    var result_holding = device_context.ReadHolding(startAddress, readCnt);
//                    seq = Array.ConvertAll(result_holding, b => b ? (byte) 1 : (byte) 0);
                    break;
                case ModbusRegisters.Input:
//                    var result_input_registers = device_context.ReadInputRegisters(startAddress, readCnt);
//                    seq = Array.ConvertAll(result_input_registers, b => b ? (byte) 1 : (byte) 0);
                    break;
            }
            
            Console.WriteLine(ByteString.CopyFrom(seq));
            return Task.FromResult(new ModbusReadResponse
            {
                Seq = ByteString.CopyFrom(seq)
            });
        }
    }

    /// <summary>
    /// Server entrypoint
    /// </summary>
    class Program
    {
        private const string Hostname = "0.0.0.0";
        private const int Port = 50051;

        public static void Main(string[] args)
        {
            var server = new Server
            {
                Services = { ModbusTCP.BindService(new ModbusImpl()) },
                Ports = { new ServerPort(Hostname, Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Modbus gRPC server listening on port " + Port);
            // Console.WriteLine("Press any key to stop the server...");
            // Console.ReadKey();

            // TODO: Fix this mock later
            while (true) { Thread.Sleep(1000); }

            server.ShutdownAsync().Wait();
        }
    }
}
