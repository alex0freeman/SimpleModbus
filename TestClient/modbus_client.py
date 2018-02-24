from __future__ import print_function

import grpc

import modbus_pb2
import modbus_pb2_grpc


def run():
  channel = grpc.insecure_channel('localhost:50051')
  stub = modbus_pb2_grpc.ModbusStub(channel)
  response = stub.GetTCP(modbus_pb2.TCPRequest(slaveId=1, functionCode=3, startAddress=3, readCnt=2))
  print("Greeter client received: " + response.seq)

if __name__ == '__main__':
    run()
