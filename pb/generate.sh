#!/bin/sh
packages/Grpc.Tools.1.8.0/tools/linux_x64/protoc -I. --csharp_out ModbusImp.Service --grpc_out ModbusImp.Service ./pb/modbus_tcp.proto --plugin=protoc-gen-grpc=packages/Grpc.Tools.1.8.0/tools/linux_x64/grpc_csharp_plugin
