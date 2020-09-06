﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Layer0.Buffers
{
    public class DataBuffer
    {
        ByteBuffer vs = new ByteBuffer();
        public static DataBuffer FromByteArray(byte[] Data)
        {
            DataBuffer dataBuffer = new DataBuffer();
            dataBuffer.vs = ByteBuffer.FromByteArray(Data);
            return dataBuffer;
        }
        public static DataBuffer FromByteBuffer(ByteBuffer buffer)
        {
            DataBuffer dataBuffer = new DataBuffer();
            dataBuffer.vs = buffer;
            return dataBuffer;
        }
        public byte[] ObtainByteArray()
        {
            return vs.GetTotalData();
        }
        public void Clear()
        {
            vs.Clear();
        }
        public byte[] ObtainByteArrayAndClear()
        {
            return vs.GetTotalDataAndClear();
        }
        public int ReadInt()
        {
            var a = vs.GetGroup();
            return BitConverter.ToInt32(a, 0);
        }
        public float ReadFloat()
        {
            var a = vs.GetGroup();
            return BitConverter.ToSingle(a, 0);
        }
        public short ReadShort()
        {
            var a = vs.GetGroup();
            return BitConverter.ToInt16(a, 0);
        }
        public ushort ReadUShort()
        {
            var a = vs.GetGroup();
            return BitConverter.ToUInt16(a, 0);
        }
        public long ReadLong()
        {
            var a = vs.GetGroup();
            return BitConverter.ToInt64(a, 0);
        }
        public ulong ReadULong()
        {
            var a = vs.GetGroup();
            return BitConverter.ToUInt64(a, 0);
        }
        public char ReadChar()
        {
            var a = vs.GetGroup();
            return BitConverter.ToChar(a, 0);
        }
        public bool ReadBool()
        {
            var a = vs.GetGroup();
            return BitConverter.ToBoolean(a, 0);
        }
        public double ReadDouble()
        {
            var a = vs.GetGroup();
            return BitConverter.ToDouble(a, 0);
        }
        public string ReadString()
        {
            var a = vs.GetGroup();
            return Encoding.UTF8.GetString(a);
        }
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
        public void WriteInt(int value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteFloat(float value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteBool(bool value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteDouble(double value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteString(string value)
        {
            vs.AppendGroup(Encoding.UTF8.GetBytes(value));
        }
        public void WriteShort(short value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteLong(long value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteUlong(ulong value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteUShort(ushort value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
        public void WriteChar(char value)
        {
            vs.AppendGroup(BitConverter.GetBytes(value));
        }
    }
}
