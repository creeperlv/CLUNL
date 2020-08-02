using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Layer0.Buffers
{
    public class ByteBuffer:IEnumerable<byte[]>
    {
        Queue<byte> buf = new Queue<byte>();
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

        public IEnumerator<byte[]> GetEnumerator()
        {
            while (buf.Count<1)
            {
                yield return GetGroup();
            }
        }
        public byte[] GetTotalData()
        {
            return buf.ToArray();
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
    }
}
