using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLUNL.Data.Layer0
{
    public struct CommandData : IConvertible
    {
        public int Command;
        public string Data;
        public CommandData(int Command, string Data)
        {
            this.Command = Command;
            this.Data = Data;
        }
        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public string ToString(IFormatProvider provider)
        {
            return string.Format("{0}|{1}", Command.ToString(), Data);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        public static implicit operator string(CommandData D)
        {
            return string.Format("{0}|{1}", D.Command.ToString(), D.Data);
        }
        public static implicit operator CommandData(string D)
        {
            var i = D.IndexOf('|');
            if (i > 0)
            {
                var C = D.Substring(0, i);
                var CD = D.Substring(i + 1);
                CommandData commandData = new CommandData(int.Parse(C), CD);
                return commandData;
            }
            else
            {
                throw new InvalidDataException();
            }
        }
    }
}
