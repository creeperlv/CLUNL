using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Convertors
{
    /// <summary>
    /// Interface for convertors that managed by ConvertorManager.
    /// </summary>
    public interface IConvertor
    {
        /// <summary>
        /// Serilize target object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        byte[] Serialize(object obj);
        /// <summary>
        /// Deserialize from given byte array to target type.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        object Deserialize(byte[] b);
        /// <summary>
        /// Must larger than 0x0C (exclude 0x0C).
        /// Used by TypeDataBuffer.
        /// </summary>
        /// <returns></returns>
        short DataHeader();
    }
}
