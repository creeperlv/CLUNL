using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLUNL.Data.Layer0.Buffers
{
    /// <summary>
    /// Basic byte buffer.
    /// </summary>
    public class ByteBuffer:IEnumerable<byte[]>
    {
        internal Queue<byte> buf = new Queue<byte>();
        /// <summary>
        /// Add a byte array to buffer. (Length will use 4 bytes).
        /// </summary>
        /// <param name="data"></param>
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
        /// <summary>
        /// Clear buffer.
        /// </summary>
        public void Clear()
        {
            buf.Clear();
        }
        /// <summary>
        /// Get enumerator, will enumerate byte array.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<byte[]> GetEnumerator()
        {
            while (buf.Count<1)
            {
                yield return GetGroup();
            }
        }
        /// <summary>
        /// Get base64 encoded string of buffer data.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Convert.ToBase64String(GetTotalData());
        }
        /// <summary>
        /// Get ByteBuffer from a base64 string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ByteBuffer FromBase64String(string str)
        {
            var b = new ByteBuffer();
            var a = Convert.FromBase64String(str);
            foreach (var item in a)
            {
                b.buf.Enqueue(item);
            }
            return b;
        }
        /// <summary>
        /// Obtain a ByteBuffer from a byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ByteBuffer FromByteArray(byte[]data)
        {
            var b = new ByteBuffer();
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
            buf.Clear();
            return arr;
        }
        public byte[] GetGroup()
        {
            byte[] Header = new byte[4];
            Header[0] = buf.Dequeue();
            Header[1] = buf.Dequeue();
            Header[2] = buf.Dequeue();
            Header[3] = buf.Dequeue();
            var TargetLength = BitConverter.ToInt32(Header, 0);
            byte[] RealContent = new byte[TargetLength];
            for (int i = 0; i < TargetLength; i++)
            {
                RealContent[i] = buf.Dequeue();
            }
            return RealContent;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            while (buf.Count < 1)
            {
                yield return GetGroup();
            }
        }
        /// <summary>
        /// Append  right ByteBuffer to left ByteBuffer as-is.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static ByteBuffer operator +(ByteBuffer L, ByteBuffer R)
        {
            foreach (var item in R.buf)
            {
                L.buf.Enqueue(item);
            }
            return L;
        }
        /// <summary>
        /// Append right ByteBuffer as an array to left ByteBuffer
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static ByteBuffer operator *(ByteBuffer L, ByteBuffer R)
        {
            L.AppendGroup(R.GetTotalData());
            return L;
        }
        public static ByteBuffer operator+(ByteBuffer L,byte R)
        {
            L.buf.Enqueue(R);
            return L;
        }
        public static ByteBuffer operator +(ByteBuffer L, byte[] R)
        {

            foreach (var item in R)
            {
                L.buf.Enqueue(item);
            }
            return L;
        }
        /// <summary>
        /// Obtain a byte array from a buffer in given length from the start of the buffer.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static byte[] operator - (ByteBuffer L,int R)
        {
            byte[] arr = new byte[R];
            for (int i = 0; i < R; i++)
            {
                arr[i] = L.buf.ElementAt(i);
            }
            return arr;
        }
        /// <summary>
        /// Obtain a series of buffer array from a given buffer.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static ByteBuffer[] operator / (ByteBuffer L,int R)
        {
            ByteBuffer[] vs = new ByteBuffer[R];
            for (int i = 0; i < R; i++)
            {
                vs[i] = L.GetGroup();
            }
            return vs;
        }
        /// <summary>
        /// Remove byte buffers in given counts and return the rest of the buffer.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static ByteBuffer operator %(ByteBuffer L,int R)
        {
            for(int i = 0; i < R; i++)
            {
                L.GetGroup();
            }
            return L;
        }
        /// <summary>
        /// Append a byte array.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static ByteBuffer operator *(ByteBuffer L, byte[] R)
        {
            L.AppendGroup(R);
            return L;
        }
        public static implicit operator ByteBuffer(byte[] Data)
        {
            ByteBuffer vs = new ByteBuffer();
            foreach (var item in Data)
            {
                vs.buf.Enqueue(item);
            }
            return vs;
        }
        public static implicit operator byte[](ByteBuffer Buffer)
        {
            return Buffer.GetTotalData();
        }
    }
}
