from __future__ import print_function

import grpc

import modbus_tcp_pb2
import modbus_tcp_pb2_grpc


def run():
    grpc_hostname = 'localhost'
    grpc_port = 50051
    modbus_ip = '127.0.0.1'
    modbus_port = 5002

    # Initialize GRPC connection
    print("Initializing gRPC connection to {}:{} ...".format(grpc_hostname, grpc_port))
    channel = grpc.insecure_channel(':'.join([grpc_hostname, str(grpc_port)]))
    stub = modbus_tcp_pb2_grpc.ModbusTCPStub(channel)

    # Connect to target device
    print("Connect to target device...")
    response = stub.ConnectDevice(modbus_tcp_pb2.ModbusConnectionRequest(
        ip=modbus_ip,
        port=modbus_port))
    print("Modbus connection status: {}".format(response.connected))

    # Read something
    print("Read Coil registers...")
    response = stub.ReadModbus(modbus_tcp_pb2.ModbusReadRequest(
        ip=modbus_ip,
        port=modbus_port,
        registerType=0,  # Coils
        startAddress=0,
        readCnt=10
        ))
    print(response.seq)
    print("Received Coil values: {}".format(str(response.seq)))

if __name__ == '__main__':
    run()
