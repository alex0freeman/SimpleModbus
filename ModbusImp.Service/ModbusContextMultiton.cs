using System;
using System.Collections.Concurrent;

namespace ModbusImp.Service
{
    public sealed class ModbusContextMultiton
    {
        /// <summary>
        /// Context of Modbus device
        /// </summary>
        private ModbusDevice<TCPContext> _modbusContext;
        public ModbusDevice<TCPContext> ModbusContext
        {
            get { return _modbusContext; }
            set { _modbusContext = value; }
        }
        
        /// <summary>
        /// Container for pool instances
        /// </summary>
        private static readonly ConcurrentDictionary<Tuple<string, ushort>, ModbusContextMultiton> _instances
            = new ConcurrentDictionary<Tuple<string, ushort>, ModbusContextMultiton>();

        /// <summary>
        /// Initialize a new device context instance
        /// </summary>
        /// <param name="key"></param>
        private ModbusContextMultiton(Tuple<string, ushort> key)
        {
            ModbusContext = new ModbusDevice<TCPContext>(new TCPContext(key.Item1, key.Item2), 0);
        }
        
        /// <summary>
        /// Get existing context instance or create new if it doesn't exists
        /// </summary>
        /// <param name="key">Tuple Hostname-Port</param>
        /// <returns>Created instance</returns>
        public static ModbusContextMultiton GetInstance(Tuple<string, ushort> key)
        {
            return _instances.GetOrAdd(key, x => new ModbusContextMultiton(x));
        }

        ~ModbusContextMultiton()
        {
            foreach (var instance in _instances.Values)
            {
                instance.ModbusContext.Disconnect();
            }
        }
    }
}