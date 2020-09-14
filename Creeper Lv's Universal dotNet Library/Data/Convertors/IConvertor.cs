using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Convertors
{
    public interface IConvertor
    {
        byte[] Serialize();
        object Deserialize(byte[] b);
    }
}
