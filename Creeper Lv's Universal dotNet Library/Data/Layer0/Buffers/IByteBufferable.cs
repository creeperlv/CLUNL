using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Layer0.Buffers
{
    /// <summary>
    /// Support converting to ByteBuffer and vice vesa
    /// </summary>
    public interface IByteBufferable
    {
        /// <summary>
        /// Convert to ByteBuffer.
        /// </summary>
        /// <param name="buffer"></param>
        void Deserialize(ByteBuffer buffer);
        /// <summary>
        /// Convert form ByteBuffer.
        /// </summary>
        /// <returns></returns>
        ByteBuffer Serialize();
    }
    /// <summary>
    /// Support converting to DataBuffer and vice vesa
    /// </summary>
    public interface IDataBufferable
    {
        /// <summary>
        /// Convert from DataBuffer
        /// </summary>
        /// <param name="buffer"></param>
        void Deserialize(DataBuffer buffer);
        /// <summary>
        /// Convert to DataBuffer
        /// </summary>
        /// <returns></returns>
        DataBuffer Serialize();
    }
    /// <summary>
    /// Support converting to TypeDataBuffer and vice vesa
    /// </summary>
    public interface ITypeDataBufferable
    {
        /// <summary>
        /// Convert from TypeDataBuffer
        /// </summary>
        /// <param name="buffer"></param>
        void Deserialize(TypeDataBuffer buffer);
        /// <summary>
        /// Convert to TypeDataBuffer.
        /// </summary>
        /// <returns></returns>
        TypeDataBuffer Serialize();
    }
}
