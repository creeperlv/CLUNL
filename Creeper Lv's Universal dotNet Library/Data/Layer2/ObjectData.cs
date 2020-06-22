using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CLUNL.Data.Layer2
{
    public class ObjectData
    {
        public static void BinSerialize(Object obj,Stream Target) {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(Target, obj);
        }
        public static object BinDeserialize(Stream Origin)
        {
            IFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(Origin);
        }
        public static void XmlSerialize(Object obj,Stream Target)
        {

        }
    }
}
