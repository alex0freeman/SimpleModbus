namespace ModbusImp
{
    enum MbErrors
    {
        IllegalFunction = 1,
        IllegalDataAddress,
        IllegalDataValue,
        SlaveDeviceFailure,
        Acknowlegment,
        SlaveDeviceBusy,
        NegativeAcknowledge,
        MemoryParityError,
        GatewayPathUnavailable,
        GatewayTargetdeviceFailedToRespond
    };

    enum MbFunctions
    {
        ReadCoils = 1,
        ReadDiscreteInputs,
        ReadHoldings,
        ReadInputs,
        WriteSingleCoil,
        WriteSingleRegister,
        WriteMultiplyCoils = 15,
        WriteMultiplyHoldingRegisters
    };
}
