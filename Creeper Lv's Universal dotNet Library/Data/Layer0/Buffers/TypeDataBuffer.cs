using CLUNL.Data.Convertors;
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
        public ByteBuffer ObtainByteBuffer()
        {
            return CoreBuffer;
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
        public Array ReadArray(Type T)
        {
            var l = BitConverter.ToInt32(CoreBuffer.GetGroup(), 0);
            Array array = Array.CreateInstance(T, l);
            for (int i = 0; i < l; i++)
            {
                if (T == typeof(int))
                {
                    array.SetValue(BitConverter.ToInt32(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (T == typeof(uint))
                {
                    array.SetValue(BitConverter.ToUInt16(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (T == typeof(short))
                {
                    array.SetValue(BitConverter.ToInt16(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (T == typeof(ushort))
                {
                    array.SetValue(BitConverter.ToUInt16(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (T == typeof(long))
                {
                    array.SetValue(BitConverter.ToInt64(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (T == typeof(ulong))
                {
                    array.SetValue(BitConverter.ToUInt64(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (T == typeof(float))
                {
                    array.SetValue(BitConverter.ToSingle(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (T == typeof(double))
                {
                    array.SetValue(BitConverter.ToDouble(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (T == typeof(bool))
                {
                    array.SetValue(BitConverter.ToBoolean(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (T == typeof(char))
                {
                    array.SetValue(BitConverter.ToChar(CoreBuffer.GetGroup(), 0), i);
                }
                else
                if (T == typeof(string))
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
            if (obj is int)
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
            else if (obj is Array)
            {
                CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Array));
                var arr = (Array)obj;
                if (arr.Length > 0)
                {
                    var item = arr.GetValue(0);
                    if (item is int)
                    {
                        CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Int));
                    }
                    else if (item is long)
                    {
                        CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Long));
                    }
                    else if (item is ulong)
                    {
                        CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.ULong));
                    }
                    else if (item is short)
                    {
                        CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Short));
                    }
                    else if (item is ushort)
                    {
                        CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.UShort));
                    }
                    else if (item is float)
                    {
                        CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Float));
                    }
                    else if (item is double)
                    {
                        CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Double));
                    }
                    else if (item is bool)
                    {
                        CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Bool));
                    }
                    else if (item is char)
                    {
                        CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Char));
                    }
                    else if (item is string)
                    {
                        CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.String));
                    }
                }
                WriteArray((Array)obj);
            }
            else
            {
                var Convertor=ConvertorManager.CurrentConvertorManager.GetConvertor(obj.GetType());
                CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.CustomData));
                CoreBuffer.AppendGroup(BitConverter.GetBytes(Convertor.DataHeader()));
                CoreBuffer.AppendGroup(Convertor.Serialize(obj));
            }
        }
        public object Read()
        {
            short flag = BitConverter.ToInt16(CoreBuffer.GetGroup(), 0);
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
                case TypeFlags.Array:
                    {
                        short DType = BitConverter.ToInt16(CoreBuffer.GetGroup(), 0);
                        Type t;
                        switch (DType)
                        {
                            case TypeFlags.Int:
                                t = typeof(int);
                                break;
                            case TypeFlags.Float:
                                t = typeof(float); break;
                            case TypeFlags.Short:
                                t = typeof(short); break;
                            case TypeFlags.UShort:
                                t = typeof(ushort); break;
                            case TypeFlags.Long:
                                t = typeof(long); break;
                            case TypeFlags.ULong:
                                t = typeof(ulong); break;
                            case TypeFlags.Double:
                                t = typeof(double); break;
                            case TypeFlags.Bool:
                                t = typeof(bool); break;
                            case TypeFlags.Char:
                                t = typeof(char); break;
                            case TypeFlags.String:
                                t = typeof(string); break;
                            case TypeFlags.Array:
                                {
                                    throw new Exception("Nested array is not supported!");

                                }
                            default:
                                throw new UndefinedTypeFlagException(flag);
                        }
                        var arr = ReadArray(t);
                        return arr;
                    }
                case TypeFlags.CustomData:
                    {
                        short DataHeader = BitConverter.ToInt16(CoreBuffer.GetGroup(), 0);
                        foreach (var item in ConvertorManager.CurrentConvertorManager.AllConvertors())
                        {
                            if (item.DataHeader() == DataHeader)
                            {
                                return item.Deserialize(CoreBuffer.GetGroup());
                            }
                            else continue;
                        }
                        throw new ConvertorNotFoundException();
                    }
                default:
                    throw new UndefinedTypeFlagException(flag);
            }
        }
        public static implicit operator TypeDataBuffer(byte[] Data)
        {
            TypeDataBuffer result = new TypeDataBuffer();
            ByteBuffer vs = new ByteBuffer();
            foreach (var item in Data)
            {
                vs.buf.Enqueue(item);
            }
            result.CoreBuffer = vs;
            return result;
        }
        public static implicit operator byte[](TypeDataBuffer Buffer)
        {
            return Buffer.ObtainByteArray();
        }
    }

    [Serializable]
    public class UndefinedTypeFlagException : Exception
    {
        public UndefinedTypeFlagException(short TypeID) : base($"Undefined TypeFlag:{TypeID}") { }
    }
    public class TypeFlags
    {
        public const short Int = 0x00;
        public const short Float = 0x01;
        public const short Double = 0x02;
        public const short Long = 0x03;
        public const short ULong = 0x04;
        public const short Short = 0x05;
        public const short UShort = 0x06;
        public const short Bool = 0x07;
        public const short Char = 0x08;
        public const short String = 0x09;
        public const short Array = 0x0A;
        public const short KVCollection = 0x0B;
        public const short CustomData = 0x0C;
    }
}
