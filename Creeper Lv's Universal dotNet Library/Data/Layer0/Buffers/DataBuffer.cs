using System;
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
        public byte[] ObtainByteArray()
        {
            return vs.GetTotalData();
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
    }
}
