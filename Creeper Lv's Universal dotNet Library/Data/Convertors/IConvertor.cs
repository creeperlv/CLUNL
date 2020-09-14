using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Convertors
{
    public interface IConvertor
    {
        byte[] Serialize(object obj);
        object Deserialize(byte[] b);
        /// <summary>
        /// Must larger than 0x0C (exclude 0x0C).
        /// Used by TypeDataBuffer.
        /// </summary>
        /// <returns></returns>
        short DataHeader();
    }
}
