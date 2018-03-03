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
        /// Current device context
        /// </summary>
        private Dictionary<Tuple<string, ushort>, ModbusDevice<IMBContext>> _device_contexts;

        ~ModbusImpl()
        {
            foreach (var ctx in this._device_contexts)
            {
                ctx.Value.Disconnect();
            }
        }
        
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
            Transport<TCPContex>.Register(1, () => new TCPContex(address, port));
            var tcp = Transport<TCPContex>.Create(1);
            _device_contexts.Add(device, new ModbusDevice<IMBContext>(tcp, 1));
            
            // Initialize connection
            _device_contexts[device].Connect();
            
            // Send connection status
            // TODO: Add connection check in library
            return Task.FromResult(new ModbusConnectionResponse { Connected = true });
        }
        
        /// <summary>
        /// Reading request to Modbus target device
        /// </summary>
        public override Task<ModbusReadResponse> ReadModbus(ModbusReadRequest request, ServerCallContext context)
        {
            var address = Convert.ToString(request.Ip);
            var port = Convert.ToUInt16(request.Port);
            var device = new Tuple<string, ushort>(address, port);
            var device_context = this._device_contexts[device];
            var registerType = request.RegisterType;
            var startAddress = Convert.ToUInt16(request.StartAddress);
            var readCnt = Convert.ToUInt16(request.ReadCnt);
            
            // Request Modbus slave 
            // TODO: Fix type conversions in library
            byte[] seq = null;
            switch (registerType)
            {
                case ModbusRegisters.Coil:
                    var resultCoil = device_context.ReadCoils(startAddress, readCnt);
                    seq = Array.ConvertAll(resultCoil, b => b ? (byte) 1 : (byte) 0);
                    break;
                case ModbusRegisters.DiscreteInput:
                    var resultInput = device_context.ReadDiscreteInputs(startAddress, readCnt);
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
