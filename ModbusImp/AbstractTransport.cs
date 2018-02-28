using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusImp
{
    public interface MBContext
    {
        void Connect();
        void Disconnect();
        int SendMsg(byte[] msg);
        int RecieveMsg(ref byte[] buff);
        byte[] BuildMessage(byte slaveId, byte functionCode, ushort startAddress, ushort readCnt);
        byte[] GetContent(byte[] fullResponce);
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
