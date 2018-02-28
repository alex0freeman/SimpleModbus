using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        ReadInputs,
        ReadHoldingRegisters,
        ReadInputRegister
    };

}
