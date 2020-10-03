using CLUNL.DirectedIO;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Layer0.Buffers
{
    /// <summary>
    /// Just operates like byte buffer.
    /// </summary>
    public class ByteBufferOverWR:IDisposable
    {
        IBaseWR CoreWR;
        public ByteBufferOverWR(IBaseWR baseWR)
        {
            CoreWR = baseWR;
        }
        public byte[] GetGroup()
        {
            //Length in Byte
            var BLength=CoreWR.Read(4, 0);
            var Length32=BitConverter.ToInt32(BLength, 0);
            return CoreWR.Read(Length32, 0);
        }
        public void AppendGroup(byte[] Data)
        {
            CoreWR.WriteBytes(Data,Data.Length,0);
            CoreWR.Flush();
        }

        public void Dispose()
        {
            CoreWR.Dispose();
        }
        /// <summary>
        /// Append  right byte to left ByteBufferOverWR as-is. Not Recommend this when using MMFWR and SreamWR when using ByteBufferOverWR to share data.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static ByteBufferOverWR operator +(ByteBufferOverWR L, byte R)
        {
            L.CoreWR.WriteBytes(new byte[] { R }, 1, 0);
            return L;
        }
        /// <summary>
        /// Append a byte array.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static ByteBufferOverWR operator *(ByteBufferOverWR L, byte[] R)
        {
            L.AppendGroup(R);
            return L;
        }
#if ENABLE_UNSTABLE
        public ByteBuffer ObtainEntireBuffer()
        {
            ByteBuffer vs = new ByteBuffer();
            int Length32;
            byte[] BLength;
            while ((BLength = CoreWR.Read(4, 0)) != null)
            {
                Length32 = BitConverter.ToInt32(BLength, 0);
                vs.AppendGroup(CoreWR.Read(Length32, 0));
            }
            return vs;
        }
#endif
    }
}
