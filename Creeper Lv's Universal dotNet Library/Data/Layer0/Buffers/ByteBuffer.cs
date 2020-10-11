using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CLUNL.Data.Layer0.Buffers
{
    /// <summary>
    /// Basic byte buffer.
    /// </summary>
    public class ByteBuffer : IEnumerable<byte[]>, IEnumerable<byte>
    {
        internal Queue<byte> buf = new Queue<byte>();
        /// <summary>
        /// Get the length of this buffer.
        /// </summary>
        public int Length { get => buf.Count; }
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
        /// Add a byte array to buffer. (Length will use 4 bytes). This method will lock buf then release it, it may cause thread blocking.
        /// </summary>
        /// <param name="data"></param>
        public void LockedAppendGroup(byte[] data)
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
            buf.Clear();
        }
        /// <summary>
        /// Get enumerator, will enumerate byte array.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<byte[]> GetEnumerator()
        {
            ByteBuffer vs = FromByteArray(buf.ToArray());
            while (vs.buf.Count < 1)
            {
                yield return vs.GetGroup();
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
        public static ByteBuffer FromByteArray(byte[] data)
        {
            var b = new ByteBuffer();
            foreach (var item in data)
            {
                b.buf.Enqueue(item);
            }
            return b;
        }
        /// <summary>
        /// Get all data from buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] GetTotalData()
        {
            return buf.ToArray();
        }
        /// <summary>
        /// Get all data from buffer then clear the buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] GetTotalDataAndClear()
        {
            var arr = buf.ToArray();
            buf.Clear();
            return arr;
        }
        /// <summary>
        /// Get a byte array.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Get a byte array with a lock to buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] LockedGetGroup()
        {
            lock (buf)
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
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            ByteBuffer vs = FromByteArray(buf.ToArray());
            while (vs.buf.Count < 1)
            {
                yield return vs.GetGroup();
            }
        }

        IEnumerator<byte> IEnumerable<byte>.GetEnumerator()
        {
            ByteBuffer vs = FromByteArray(buf.ToArray());
            while (vs.buf.Count < 1)
            {
                yield return buf.Dequeue();
            }
        }
        /// <summary>
        /// Get the index of first given byte.(Slow performance)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(byte item)
        {
            var a = buf.ToArray();
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == item) return i;
            }
            throw new ArgumentOutOfRangeException();
        }
        /// <summary>
        /// Insert a byte into given index. (Slow performance)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, byte item)
        {
            List<byte> ls = new List<byte>(buf);
            ls.Insert(index, item);
            buf = new Queue<byte>(ls);
        }
        /// <summary>
        /// Remove byte at given index.(Slow performance)
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            List<byte> ls = new List<byte>(buf);
            ls.RemoveAt(index);
            buf = new Queue<byte>(ls);
        }
        /// <summary>
        /// Add a byte to the end of the buffer.
        /// </summary>
        /// <param name="item"></param>
        public void Add(byte item)
        {
            buf.Enqueue(item);
        }
        /// <summary>
        /// Determines whether the buffer contains given byte. (Slow performance)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(byte item)
        {
            List<byte> ls = new List<byte>(buf);
            return ls.Contains(item);
        }
        /// <summary>
        /// Copy the content of the buffer to given array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(byte[] array, int arrayIndex)
        {
            buf.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Remove the given byte.(Slow performance)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(byte item)
        {
            List<byte> ls = new List<byte>(buf);
            return ls.Remove(item);
        }

        /// <summary>
        /// Obtain the first byte of the buffer and remove it.
        /// </summary>
        public byte α { get => buf.Dequeue(); }
        /// <summary>
        /// Obtain the first byte of the buffer and remove it.
        /// </summary>
        public byte First { get => buf.Dequeue(); }

        /// <summary>
        /// Slow performance
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte this[int index] { get => buf.ToArray()[index]; set =>buf=new Queue<byte>(buf.ToArray()[index] = value); }

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
        /// <summary>
        /// Append a byte to ByteBuffer without treating it as an array.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static ByteBuffer operator +(ByteBuffer L, byte R)
        {
            L.buf.Enqueue(R);
            return L;
        }
        /// <summary>
        /// Same as AppendGroup(byte[])/
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
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
        public static byte[] operator -(ByteBuffer L, int R)
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
        public static ByteBuffer[] operator /(ByteBuffer L, int R)
        {
            ByteBuffer[] vs = new ByteBuffer[R];
            for (int i = 0; i < R; i++)
            {
                vs[i] = L.GetGroup();
            }
            return vs;
        }
        /// <summary>
        /// Remove byte arraies in given counts and return the rest of the buffer.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static ByteBuffer operator %(ByteBuffer L, int R)
        {
            for (int i = 0; i < R; i++)
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
        /// <summary>
        /// Convert from byte array to ByteBuffer.
        /// </summary>
        /// <param name="Data"></param>
        public static implicit operator ByteBuffer(byte[] Data)
        {
            ByteBuffer vs = new ByteBuffer();
            foreach (var item in Data)
            {
                vs.buf.Enqueue(item);
            }
            return vs;
        }
        /// <summary>
        /// Convert from ByteBuffer to byte array.
        /// </summary>
        /// <param name="Buffer"></param>
        public static implicit operator byte[](ByteBuffer Buffer)
        {
            return Buffer.GetTotalData();
        }
    }
}
