using CLUNL.Data.Convertors;
using System;
using System.Text;

namespace CLUNL.Data.Layer0.Buffers
{
    /// <summary>
    /// A buffer that allows to directly write any primitive data and will auto convert them.
    /// </summary>
    public class TypeDataBuffer
    {
        /// <summary>
        /// Core byte buffer.
        /// </summary>
        public ByteBuffer CoreBuffer = new ByteBuffer();
        /// <summary>
        /// Get the length of this buffer.
        /// </summary>
        public int Length { get => CoreBuffer.Length; }
        /// <summary>
        /// Obtain a TypeDataBuffer from a byte array.
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static TypeDataBuffer FromByteArray(byte[] Data)
        {
            TypeDataBuffer Buffer = new TypeDataBuffer();
            Buffer.CoreBuffer = ByteBuffer.FromByteArray(Data);
            return Buffer;
        }
        /// <summary>
        /// Obtain a TypeDataBuffer from a ByteBuffer.
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        public static TypeDataBuffer FromByteBuffer(ByteBuffer vs)
        {
            TypeDataBuffer Buffer = new TypeDataBuffer();
            Buffer.CoreBuffer = vs;
            return Buffer;
        }
        /// <summary>
        /// Obtain the core ByteBuffer.
        /// </summary>
        /// <returns></returns>
        public ByteBuffer ObtainByteBuffer()
        {
            return CoreBuffer;
        }
        /// <summary>
        /// Obtain a byte array of buffered data.
        /// </summary>
        /// <returns></returns>
        public byte[] ObtainByteArray()
        {
            return CoreBuffer.GetTotalData();
        }
        /// <summary>
        /// Clear the buffer.
        /// </summary>
        public void Clear()
        {
            CoreBuffer.Clear();
        }
        /// <summary>
        /// Obtain a byte array then clear the buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] ObtainByteArrayAndClear()
        {
            return CoreBuffer.GetTotalDataAndClear();
        }
        /// <summary>
        /// Read an array with primitive data. Nested array is <b>not</b> supported.
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Write an array with primitive data. Nested array is <b>not</b> supported.
        /// </summary>
        /// <param name="array"></param>
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
        /// <summary>
        /// Write a data which can be primitive or data with registered convertor.
        /// </summary>
        /// <param name="obj"></param>
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
                var Convertor = ConvertorManager.CurrentConvertorManager.GetConvertor(obj.GetType());
                CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.CustomData));
                CoreBuffer.AppendGroup(BitConverter.GetBytes(Convertor.DataHeader()));
                CoreBuffer.AppendGroup(Convertor.Serialize(obj));
            }
        }
        /// <summary>
        /// Read a data which can be primitive or data with registered convertor.
        /// </summary>
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
        /// <summary>
        /// Convert from byte array to TypeDataBuffer.
        /// </summary>
        /// <param name="Data"></param>
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
        /// <summary>
        /// Convert from TypeDataBuffer to byte array.
        /// </summary>
        /// <param name="Buffer"></param>
        public static implicit operator byte[](TypeDataBuffer Buffer)
        {
            return Buffer.ObtainByteArray();
        }
    }
    /// <summary>
    /// Throw when the type flag is undefined.
    /// </summary>
    [Serializable]
    public class UndefinedTypeFlagException : Exception
    {
        /// <summary>
        /// Throw when the type flag is undefined.
        /// </summary>
        public UndefinedTypeFlagException(short TypeID) : base($"Undefined TypeFlag:{TypeID}") { }
    }
    /// <summary>
    /// Known type flags.
    /// </summary>
    public class TypeFlags
    {
        /// <summary>
        /// Int32, int
        /// </summary>
        public const short Int = 0x00;
        /// <summary>
        /// Single, float
        /// </summary>
        public const short Float = 0x01;
        /// <summary>
        /// Double, double
        /// </summary>
        public const short Double = 0x02;
        /// <summary>
        /// Int64, long
        /// </summary>
        public const short Long = 0x03;
        /// <summary>
        /// Unsigned Int64, ulong
        /// </summary>
        public const short ULong = 0x04;
        /// <summary>
        /// Int16, short
        /// </summary>
        public const short Short = 0x05;
        /// <summary>
        /// Unsigned Int16, ushort
        /// </summary>
        public const short UShort = 0x06;
        /// <summary>
        /// Boolean, bool
        /// </summary>
        public const short Bool = 0x07;
        /// <summary>
        /// Character, char
        /// </summary>
        public const short Char = 0x08;
        /// <summary>
        /// String, string
        /// </summary>
        public const short String = 0x09;
        /// <summary>
        /// Array
        /// </summary>
        public const short Array = 0x0A;
        /// <summary>
        /// KVCollection, not implemented yet.
        /// </summary>
        public const short KVCollection = 0x0B;
        /// <summary>
        /// Customed data, requies its convertor to be registered.
        /// </summary>
        public const short CustomData = 0x0C;
    }
}
