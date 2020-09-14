using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Convertors
{
    public class ConvertorManager
    {
        public static ConvertorManager CurrentConvertorManager = new ConvertorManager();
        Dictionary<Type, IConvertor> Convertors = new Dictionary<Type, IConvertor>();
        ConvertorManager()
        {

        }
        public void RegisterConvertor(Type T,IConvertor Convertor)
        {
            if (Convertors.ContainsKey(T)) Convertors[T] = Convertor; else Convertors.Add(T, Convertor);
        }
        public void UnregisterConvertor(Type T)
        {
            Convertors.Remove(T);
        }
        public IConvertor GetConvertor(Type T)
        {
            return Convertors[T];
        }
    }
}
