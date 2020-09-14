using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Layer0.Buffers
{
    public interface IByteBufferable
    {
        void Deserialize(ByteBuffer buffer);
        ByteBuffer Serialize();
    }
    public interface IDataBufferable
    {
        void Deserialize(DataBuffer buffer);
        DataBuffer Serialize();
    }
    public interface ITypeDataBufferable
    {
        void Deserialize(TypeDataBuffer buffer);
        TypeDataBuffer Serialize();
    }
}
