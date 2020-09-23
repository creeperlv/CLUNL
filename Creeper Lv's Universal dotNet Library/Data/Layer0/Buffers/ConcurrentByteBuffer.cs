using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Layer0.Buffers
{
    public class ConcurrentByteBuffer : IEnumerable<byte[]>
    {
        ConcurrentQueue<byte> buf = new ConcurrentQueue<byte>();
        public void AppendGroup(byte[] data)
        {
            var H = BitConverter.GetBytes(data.Length);
            foreach (var item in H)
            {
                buf.Enqueue(item);
            }
            foreach (var item in data)
            {
                buf.Enqueue(item);
            }
        }
        public void Clear()
        {
            buf = new ConcurrentQueue<byte>();
        }
        public IEnumerator<byte[]> GetEnumerator()
        {
            while (buf.Count < 1)
            {
                yield return GetGroup();
            }
        }
        public override string ToString()
        {
            return Convert.ToBase64String(GetTotalData());
        }
        public static ConcurrentByteBuffer FromBase64String(string str)
        {
            var b = new ConcurrentByteBuffer();
            var a = Convert.FromBase64String(str);
            foreach (var item in a)
            {
                b.buf.Enqueue(item);
            }
            return b;
        }
        public static ConcurrentByteBuffer FromByteArray(byte[] data)
        {
            var b = new ConcurrentByteBuffer();
            foreach (var item in data)
            {
                b.buf.Enqueue(item);
            }
            return b;
        }
        public byte[] GetTotalData()
        {
            return buf.ToArray();
        }
        public byte[] GetTotalDataAndClear()
        {
            var arr = buf.ToArray();
            Clear();
            return arr;
        }
        public byte[] GetGroup()
        {
            byte[] Header = new byte[4];
            buf.TryDequeue(out Header[0]);
            buf.TryDequeue(out Header[1]);
            buf.TryDequeue(out Header[2]);
            buf.TryDequeue(out Header[3]);
            var TargetLength = BitConverter.ToInt32(Header, 0);
            byte[] RealContent = new byte[TargetLength];
            for (int i = 0; i < TargetLength; i++)
            {
                buf.TryDequeue(out RealContent[i]);
            }
            return RealContent;
        }
        public static ConcurrentByteBuffer operator +(ConcurrentByteBuffer L, ConcurrentByteBuffer R)
        {
            lock (L)
            {
                foreach (var item in R.buf)
                {
                    L.buf.Enqueue(item);
                }
            }
            return L;
        }
        public static ConcurrentByteBuffer operator +(ConcurrentByteBuffer L, byte R)
        {
            lock (L)
            {
                L.buf.Enqueue(R);
            }
            return L;
        }
        public static ConcurrentByteBuffer operator +(ConcurrentByteBuffer L, byte[] R)
        {
            lock (L)
            {
                foreach (var item in R)
                {
                    L.buf.Enqueue(item);
                }
            }
            return L;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            while (buf.Count < 1)
            {
                yield return GetGroup();
            }
        }
    }
}
