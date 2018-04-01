# Simple Modbus

Service that exploses Modbus TCP protocol to gRPC.

## Description

Service represented with two components:
+ ModbusImp
  Library that implements Modbus access to target devices
+ ModbusImp.Service
  gRPC service used as middleware between web client and target device

## Usage

1. Run gRPC service in docker:
```
  docker build . -t modbus-tcp-service
  docker run -t --rm -p 50051:50051 modbus-tcp-service
```

2. Create gRPC client using our API (described in protobuf files) to connect and query Modbus devices from network.
That can be done with with simple python client runned on localhost:
```
  cd TestClient
  pipenv shell
  python -m grpc_tools.protoc -I../pb --python_out=. --grpc_python_out=. ../pb/modbus_tcp.proto
  python modbus_client.py
```

You also can use project tests based on NUnit framework. There is additional overhead with ModbusImp.Test (described in comments to `Test.cs`): standalone Modbus TCP slave is required.
