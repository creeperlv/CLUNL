using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Layer0.Buffers
{
    /// <summary>
    /// ConcurrentByteBuffer that is thread safe.
    /// </summary>
    public class ConcurrentByteBuffer : IEnumerable<byte[]>
    {
        ConcurrentQueue<byte> buf = new ConcurrentQueue<byte>();
        /// <summary>
        /// Same as ByteBuffer.AppendGroup(byte[]).
        /// </summary>
        /// <param name="data"></param>
        public void AppendGroup(byte[] data)
        {
            lock (buf)
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
        }
        /// <summary>
        /// Clear buffer.
        /// </summary>
        public void Clear()
        {
            buf = new ConcurrentQueue<byte>();
        }
        /// <summary>
        /// Get enumerator to enumerate byte arrays.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<byte[]> GetEnumerator()
        {
            while (buf.Count < 1)
            {
                yield return GetGroup();
            }
        }
        /// <summary>
        /// Convert buffer data to Base64 string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Convert.ToBase64String(GetTotalData());
        }
        /// <summary>
        /// Obtain ConcurrentByteBuffer from base64 string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Obtain ConcurrentByteBuffer from a byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ConcurrentByteBuffer FromByteArray(byte[] data)
        {
            var b = new ConcurrentByteBuffer();
            foreach (var item in data)
            {
                b.buf.Enqueue(item);
            }
            return b;
        }
        /// <summary>
        /// Obtain byte array from buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] GetTotalData()
        {
            return buf.ToArray();
        }
        /// <summary>
        /// Obtain byte array then clear buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] GetTotalDataAndClear()
        {
            var arr = buf.ToArray();
            Clear();
            return arr;
        }
        /// <summary>
        /// Get a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetGroup()
        {
            lock (buf)
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
        }
        /// <summary>
        /// Connect R to L (L is in front of R)
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Append R to the buffer without treating it as an array.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static ConcurrentByteBuffer operator +(ConcurrentByteBuffer L, byte R)
        {
            lock (L)
            {
                L.buf.Enqueue(R);
            }
            return L;
        }
        /// <summary>
        /// Append an array to the buffer without using AppendGroup(byte[]); (will not add length field in front of the array).
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
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
