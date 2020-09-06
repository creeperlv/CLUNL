using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Layer0.Buffers
{
    public class TypeDataBuffer
    {
        public ByteBuffer CoreBuffer { get; } = new ByteBuffer();
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
        public int ReadInt()
        {
            var a = CoreBuffer.GetGroup();
            return BitConverter.ToInt32(a, 0);
        }
        public float ReadFloat()
        {
            var a = CoreBuffer.GetGroup();
            return BitConverter.ToSingle(a, 0);
        }
        public bool ReadBool()
        {
            var a = CoreBuffer.GetGroup();
            return BitConverter.ToBoolean(a, 0);
        }
        public double ReadDouble()
        {
            var a = CoreBuffer.GetGroup();
            return BitConverter.ToDouble(a, 0);
        }
        public string ReadString()
        {
            var a = CoreBuffer.GetGroup();
            return Encoding.UTF8.GetString(a);
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
        public void WriteInt(int value)
        {
            CoreBuffer.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteFloat(float value)
        {
            CoreBuffer.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteShort(short value)
        {
            CoreBuffer.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteLong(long value)
        {
            CoreBuffer.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteUlong(ulong value)
        {
            CoreBuffer.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteUShort(ushort value)
        {
            CoreBuffer.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteBool(bool value)
        {
            CoreBuffer.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteChar(char value)
        {
            CoreBuffer.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteDouble(double value)
        {
            CoreBuffer.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteString(string value)
        {
            CoreBuffer.AppendGroup(Encoding.UTF8.GetBytes(value));
        }
        public void WriteObj(object obj)
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
            if (obj is short)
            {
                CoreBuffer.AppendGroup(BitConverter.GetBytes(TypeFlags.Short));
                CoreBuffer.AppendGroup(BitConverter.GetBytes((short)obj));
            }
        }
    }

    public class TypeFlags
    {
        public static byte Int { get; } = 0x00;
        public static byte Float { get; } = 0x01;
        public static byte Double { get; } = 0x02;
        public static byte Long { get; } = 0x03;
        public static byte ULong { get; } = 0x04;
        public static byte Short { get; } = 0x05;
        public static byte UShort { get; } = 0x06;
        public static byte Bool { get; } = 0x07;
        public static byte Char { get; } = 0x08;
    }
}
