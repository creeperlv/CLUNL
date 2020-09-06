using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Layer0.Buffers
{
    public class TypeDataBuffer
    {
        public ByteBuffer CoreBuffer = new ByteBuffer();
        public static TypeDataBuffer FromByteArray(byte[] Data)
        {
            TypeDataBuffer Buffer = new TypeDataBuffer();
            Buffer.CoreBuffer = ByteBuffer.FromByteArray(Data);
            return Buffer;
        }
        public static TypeDataBuffer FromByteBuffer(ByteBuffer vs)
        {
            TypeDataBuffer Buffer = new TypeDataBuffer();
            Buffer.CoreBuffer = vs;
            return Buffer;
        }
        public byte[] ObtainByteArray()
        {
            return CoreBuffer.GetTotalData();
        }
        public void Clear()
        {
            CoreBuffer.Clear();
        }
        public byte[] ObtainByteArrayAndClear()
        {
            return CoreBuffer.GetTotalDataAndClear();
        }
        public Array ReadArray<T>()
        {
            var l = BitConverter.ToInt32(CoreBuffer.GetGroup(), 0);
            Array array = Array.CreateInstance(typeof(T), l);
            for (int i = 0; i < l; i++)
            {
                if (typeof(T) == typeof(int))
                {
                    array.SetValue(BitConverter.ToInt32(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(uint))
                {
                    array.SetValue(BitConverter.ToUInt16(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(short))
                {
                    array.SetValue(BitConverter.ToInt16(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(ushort))
                {
                    array.SetValue(BitConverter.ToUInt16(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(long))
                {
                    array.SetValue(BitConverter.ToInt64(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(ulong))
                {
                    array.SetValue(BitConverter.ToUInt64(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(float))
                {
                    array.SetValue(BitConverter.ToSingle(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(double))
                {
                    array.SetValue(BitConverter.ToDouble(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(bool))
                {
                    array.SetValue(BitConverter.ToBoolean(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(char))
                {
                    array.SetValue(BitConverter.ToChar(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(string))
                {
                    array.SetValue(Encoding.UTF8.GetString(CoreBuffer.GetGroup()), i);
                }
            }
            return null;
        }
        public void WriteArray(Array array)
        {
            CoreBuffer.AppendGroup(BitConverter.GetBytes(array.Length));
            foreach (var item in array)
            {
                if (item is int)
                {
                    CoreBuffer.AppendGroup(BitConverter.GetBytes((int)item));
                }
                else if (item is uint)
                {
                    CoreBuffer.AppendGroup(BitConverter.GetBytes((uint)item));
                }
                else if (item is float)
                {
                    CoreBuffer.AppendGroup(BitConverter.GetBytes((float)item));
                }
                else if (item is bool)
                {
                    CoreBuffer.AppendGroup(BitConverter.GetBytes((bool)item));
                }
                else if (item is double)
                {
                    CoreBuffer.AppendGroup(BitConverter.GetBytes((double)item));
                }
                else if (item is char)
                {
                    CoreBuffer.AppendGroup(BitConverter.GetBytes((char)item));
                }
                else if (item is long)
                {
                    CoreBuffer.AppendGroup(BitConverter.GetBytes((long)item));
                }
                else if (item is short)
                {
                    CoreBuffer.AppendGroup(BitConverter.GetBytes((short)item));
                }
                else if (item is ulong)
                {
                    CoreBuffer.AppendGroup(BitConverter.GetBytes((ulong)item));
                }
                else if (item is ushort)
                {
                    CoreBuffer.AppendGroup(BitConverter.GetBytes((ushort)item));
                }
                else if (item is string)
                {
                    CoreBuffer.AppendGroup(Encoding.UTF8.GetBytes((string)item));
                }
            }
        }
        public void Write(object obj)
        {
            if(obj is int)
            {
                CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Int));
                CoreBuffer.AppendGroup(BitConverter.GetBytes((int)obj));
            }
            else
            if (obj is long)
            {
                CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Long));
                CoreBuffer.AppendGroup(BitConverter.GetBytes((long)obj));
            }
            else
            if (obj is ulong)
            {
                CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.ULong));
                CoreBuffer.AppendGroup(BitConverter.GetBytes((ulong)obj));
            }
            else
            if (obj is short)
            {
                CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Short));
                CoreBuffer.AppendGroup(BitConverter.GetBytes((short)obj));
            }
            else
            if (obj is ushort)
            {
                CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.UShort));
                CoreBuffer.AppendGroup(BitConverter.GetBytes((ushort)obj));
            }
            else
            if (obj is float)
            {
                CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Float));
                CoreBuffer.AppendGroup(BitConverter.GetBytes((float)obj));
            }
            else
            if (obj is double)
            {
                CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Double));
                CoreBuffer.AppendGroup(BitConverter.GetBytes((double)obj));
            }
            else
            if (obj is bool)
            {
                CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Bool));
                CoreBuffer.AppendGroup(BitConverter.GetBytes((bool)obj));
            }
            else
            if (obj is char)
            {
                CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Char));
                CoreBuffer.AppendGroup(BitConverter.GetBytes((char)obj));
            }
            else
            if (obj is string)
            {
                CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.String));
                CoreBuffer.AppendGroup(Encoding.UTF8.GetBytes((string)obj));
            }
        }
        public object Read()
        {
            short flag =BitConverter.ToInt16( CoreBuffer.GetGroup(),0);
            switch (flag)
            {
                case TypeFlags.Int:
                    return BitConverter.ToInt32(CoreBuffer.GetGroup(), 0);
                case TypeFlags.Float:
                    return BitConverter.ToSingle(CoreBuffer.GetGroup(), 0);
                case TypeFlags.Short:
                    return BitConverter.ToInt16(CoreBuffer.GetGroup(), 0);
                case TypeFlags.UShort:
                    return BitConverter.ToUInt16(CoreBuffer.GetGroup(), 0);
                case TypeFlags.Long:
                    return BitConverter.ToInt64(CoreBuffer.GetGroup(), 0);
                case TypeFlags.ULong:
                    return BitConverter.ToUInt64(CoreBuffer.GetGroup(), 0);
                case TypeFlags.Double:
                    return BitConverter.ToDouble(CoreBuffer.GetGroup(), 0);
                case TypeFlags.Bool:
                    return BitConverter.ToBoolean(CoreBuffer.GetGroup(), 0);
                case TypeFlags.Char:
                    return BitConverter.ToChar(CoreBuffer.GetGroup(), 0);
                case TypeFlags.String:
                    return Encoding.UTF8.GetString(CoreBuffer.GetGroup());
                default:
                    throw new UndefinedTypeFlagException(flag);
            }
        }
    }

    [Serializable]
    public class UndefinedTypeFlagException : Exception
    {
        public UndefinedTypeFlagException(short TypeID):base($"Undefined TypeFlag:{TypeID}") { }
    }
    public class TypeFlags
    {
        public const short Int= 0x00;
        public const short Float = 0x01;
        public const short Double = 0x02;
        public const short Long = 0x03;
        public const short ULong = 0x04;
        public const short Short = 0x05;
        public const short UShort = 0x06;
        public const short Bool = 0x07;
        public const short Char = 0x08;
        public const short String = 0x09;
    }
}
