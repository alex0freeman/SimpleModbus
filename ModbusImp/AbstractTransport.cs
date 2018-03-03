using System;
using System.Collections.Generic;

namespace ModbusImp
{
    public interface MBContext
    {
        void Connect();
        void Disconnect();
        int SendMsg(byte[] msg);
        int RecieveMsg(ref byte[] buff);
        byte[] BuildMessage(byte slaveId, byte functionCode, byte[] data);
        byte[] GetContent(byte[] fullResponse, int expectedBytes);
        int GetHeader(); 
    }

    public class Transport<T>
    {
        private Transport() { }

        static readonly Dictionary<int, Func<T>> _dict
             = new Dictionary<int, Func<T>>();

        public static T Create(int id)
        {
            Func<T> constructor = null;
            if (_dict.TryGetValue(id, out constructor))
                return constructor();

            throw new ArgumentException("No type registered for this id");
        }

        public static void Register(int id, Func<T> ctor)
        {
            _dict.Add(id, ctor);
        }
    }
}
