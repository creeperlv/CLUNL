using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Layer0.Buffers
{
    public class DataBuffer
    {
        ByteBuffer vs = new ByteBuffer();
        /// <summary>
        /// Generate a DataBuffer from a byte array.
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static DataBuffer FromByteArray(byte[] Data)
        {
            DataBuffer dataBuffer = new DataBuffer();
            dataBuffer.vs = ByteBuffer.FromByteArray(Data);
            return dataBuffer;
        }
        /// <summary>
        /// Generate a DataBuffer from a ByteBuffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static DataBuffer FromByteBuffer(ByteBuffer buffer)
        {
            DataBuffer dataBuffer = new DataBuffer();
            dataBuffer.vs = buffer;
            return dataBuffer;
        }
        /// <summary>
        /// Get Byte Array of current buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] ObtainByteArray()
        {
            return vs.GetTotalData();
        }
        /// <summary>
        /// Clear the buffer.
        /// </summary>
        public void Clear()
        {
            vs.Clear();
        }
        /// <summary>
        /// Get byte array and then clear.
        /// </summary>
        /// <returns></returns>
        public byte[] ObtainByteArrayAndClear()
        {
            return vs.GetTotalDataAndClear();
        }
        /// <summary>
        /// Read an int value.
        /// </summary>
        /// <returns></returns>
        public int ReadInt()
        {
            var a = vs.GetGroup();
            return BitConverter.ToInt32(a, 0);
        }
        /// <summary>
        /// Read a float value.
        /// </summary>
        /// <returns></returns>
        public float ReadFloat()
        {
            var a = vs.GetGroup();
            return BitConverter.ToSingle(a, 0);
        }
        /// <summary>
        /// Read a shor value
        /// </summary>
        /// <returns></returns>
        public short ReadShort()
        {
            var a = vs.GetGroup();
            return BitConverter.ToInt16(a, 0);
        }
        /// <summary>
        /// Read a unsigned value.
        /// </summary>
        /// <returns></returns>
        public ushort ReadUShort()
        {
            var a = vs.GetGroup();
            return BitConverter.ToUInt16(a, 0);
        }
        /// <summary>
        /// Read a long value.
        /// </summary>
        /// <returns></returns>
        public long ReadLong()
        {
            var a = vs.GetGroup();
            return BitConverter.ToInt64(a, 0);
        }
        /// <summary>
        /// Read a unsigned long value.
        /// </summary>
        /// <returns></returns>
        public ulong ReadULong()
        {
            var a = vs.GetGroup();
            return BitConverter.ToUInt64(a, 0);
        }
        /// <summary>
        /// Read a char value.
        /// </summary>
        /// <returns></returns>
        public char ReadChar()
        {
            var a = vs.GetGroup();
            return BitConverter.ToChar(a, 0);
        }
        /// <summary>
        /// Read a boolean value.
        /// </summary>
        /// <returns></returns>
        public bool ReadBool()
        {
            var a = vs.GetGroup();
            return BitConverter.ToBoolean(a, 0);
        }
        /// <summary>
        /// Read a double value.
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            var a = vs.GetGroup();
            return BitConverter.ToDouble(a, 0);
        }
        /// <summary>
        /// Read a string.
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            var a = vs.GetGroup();
            return Encoding.UTF8.GetString(a);
        }
        /// <summary>
        /// Read an array that only contains primitive types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Array ReadArray<T>()
        {
            var l = BitConverter.ToInt32(vs.GetGroup(), 0);
            Array array = Array.CreateInstance(typeof(T), l) ;
            for (int i = 0; i < l; i++)
            {
                if (typeof(T) == typeof(int))
                {
                    array.SetValue(BitConverter.ToInt32(vs.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(uint))
                {
                    array.SetValue(BitConverter.ToUInt16(vs.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(short))
                {
                    array.SetValue(BitConverter.ToInt16(vs.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(ushort))
                {
                    array.SetValue(BitConverter.ToUInt16(vs.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(long))
                {
                    array.SetValue(BitConverter.ToInt64(vs.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(ulong))
                {
                    array.SetValue(BitConverter.ToUInt64(vs.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(float))
                {
                    array.SetValue(BitConverter.ToSingle(vs.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(double))
                {
                    array.SetValue(BitConverter.ToDouble(vs.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(bool))
                {
                    array.SetValue(BitConverter.ToBoolean(vs.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(char))
                {
                    array.SetValue(BitConverter.ToChar(vs.GetGroup(), 0), i);
                }
                else
                if (typeof(T) == typeof(string))
                {
                    array.SetValue(Encoding.UTF8.GetString(vs.GetGroup()), i);
                }
            }
            return null;
        }
        /// <summary>
        /// Write an array with primitive data.
        /// </summary>
        /// <param name="array"></param>
        public void WriteArray(Array array)
        {
            vs.AppendGroup(BitConverter.GetBytes(array.Length));
            foreach (var item in array)
            {
                if (item is int)
                {
                    vs.AppendGroup(BitConverter.GetBytes((int)item));
                }else if (item is uint)
                {
                    vs.AppendGroup(BitConverter.GetBytes((uint)item));
                }
                else if (item is float)
                {
                    vs.AppendGroup(BitConverter.GetBytes((float)item));
                }
                else if (item is bool)
                {
                    vs.AppendGroup(BitConverter.GetBytes((bool)item));
                }
                else if (item is double)
                {
                    vs.AppendGroup(BitConverter.GetBytes((double)item));
                }
                else if (item is char)
                {
                    vs.AppendGroup(BitConverter.GetBytes((char)item));
                }
                else if (item is long)
                {
                    vs.AppendGroup(BitConverter.GetBytes((long)item));
                }
                else if (item is short)
                {
                    vs.AppendGroup(BitConverter.GetBytes((short)item));
                }
                else if (item is ulong)
                {
                    vs.AppendGroup(BitConverter.GetBytes((ulong)item));
                }
                else if (item is ushort)
                {
                    vs.AppendGroup(BitConverter.GetBytes((ushort)item));
                }
                else if (item is string)
                {
                    vs.AppendGroup(Encoding.UTF8.GetBytes((string)item));
                }
            }
        }
        /// <summary>
        /// Write an int value.
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt(int value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Write a float value.
        /// </summary>
        /// <param name="value"></param>
        public void WriteFloat(float value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Write a boolean value.
        /// </summary>
        /// <param name="value"></param>
        public void WriteBool(bool value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Write a double value.
        /// </summary>
        /// <param name="value"></param>
        public void WriteDouble(double value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Write a string.
        /// </summary>
        /// <param name="value"></param>
        public void WriteString(string value)
        {
            vs.AppendGroup(Encoding.UTF8.GetBytes(value));
        }
        /// <summary>
        /// Write a short value.
        /// </summary>
        /// <param name="value"></param>
        public void WriteShort(short value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Write a long value.
        /// </summary>
        /// <param name="value"></param>
        public void WriteLong(long value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Write a long value that is unsigned.
        /// </summary>
        /// <param name="value"></param>
        public void WriteUlong(ulong value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Write a short value that is unsigned.
        /// </summary>
        /// <param name="value"></param>
        public void WriteUShort(ushort value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Write a char.
        /// </summary>
        /// <param name="value"></param>
        public void WriteChar(char value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Convert from byte array to DataBuffer.
        /// </summary>
        /// <param name="Data"></param>
        public static implicit operator DataBuffer(byte[] Data)
        {
            DataBuffer result = new DataBuffer();
               ByteBuffer vs = new ByteBuffer();
            foreach (var item in Data)
            {
                vs.buf.Enqueue(item);
            }
            result.vs = vs;
            return result;
        }
        /// <summary>
        /// Convert from DataBuffer to a byte array.
        /// </summary>
        /// <param name="Buffer"></param>
        public static implicit operator byte[](DataBuffer Buffer)
        {
            return Buffer.ObtainByteArray();
        }
    }
}
