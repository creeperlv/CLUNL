using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CLUNL.Data.Layer2
{
    /// <summary>
    /// <b>[Obsoleted]</b> Replaced by ByteBuffer, DataBuffer and TypeDataBuffer.
    /// </summary>
    [Obsolete]
    public class ObjectData
    {
        /// <summary>
        /// Serialize object to binary and write to target stream.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Target"></param>
        public static void BinSerialize(Object obj,Stream Target) {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(Target, obj);
        }
        /// <summary>
        /// Deserialize an object from given stream.
        /// </summary>
        /// <param name="Origin"></param>
        /// <returns></returns>
        public static object BinDeserialize(Stream Origin)
        {
            IFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(Origin);
        }
        //public static void XmlSerialize(Object obj,Stream Target)
        //{

        //}
    }
}
