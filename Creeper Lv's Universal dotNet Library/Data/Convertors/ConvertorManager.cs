using System;
using System.Collections.Generic;
using static System.Collections.Generic.Dictionary<System.Type, CLUNL.Data.Convertors.IConvertor>;

namespace CLUNL.Data.Convertors
{
    /// <summary>
    /// Manages convertors
    /// </summary>
    public class ConvertorManager
    {
        /// <summary>
        /// Only allows on public manager instance and TypeDataBuffer will only find this manager.
        /// </summary>
        public static ConvertorManager CurrentConvertorManager = new ConvertorManager();
        Dictionary<Type, IConvertor> Convertors = new Dictionary<Type, IConvertor>();
        ConvertorManager()
        {

        }
        /// <summary>
        /// Get all convertors
        /// </summary>
        /// <returns></returns>
        public ValueCollection AllConvertors()
        {
            return Convertors.Values;
        }
        /// <summary>
        /// Register a convertor.
        /// </summary>
        /// <param name="T"></param>
        /// <param name="Convertor"></param>
        public void RegisterConvertor(Type T, IConvertor Convertor)
        {
            if (Convertors.ContainsKey(T)) Convertors[T] = Convertor; else Convertors.Add(T, Convertor);
        }
        /// <summary>
        /// Unregister a convertor.
        /// </summary>
        /// <param name="T"></param>
        public void UnregisterConvertor(Type T)
        {
            Convertors.Remove(T);
        }
        /// <summary>
        /// Find a convertor for given type T.
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public IConvertor GetConvertor(Type T)
        {
            return Convertors[T];
        }
    }

    /// <summary>
    /// An exception represents manager have no needed convertor.
    /// </summary>
    [Serializable]
    public class ConvertorNotFoundException : Exception
    {
        /// <summary>
        /// An exception represents manager have no needed convertor.
        /// </summary>
        public ConvertorNotFoundException() : base("Cannot find target convertor.") { }
    }
}
