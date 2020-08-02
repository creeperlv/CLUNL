using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
namespace CLUNL.Pipeline
{
    ///<summary>
    /// A manifest that describes which units the processor will use.
    ///</summart>
    public class ProcessUnitManifest : IList<Type>
    {
        List<Type> Data = new List<Type>();
        public Type this[int index] { get => Data[index]; set => Data[index] = value; }

        public int Count => Data.Count;

        public bool IsReadOnly => true;

        public void Add(Type item)
        {
            Data.Add(item);
        }
        public static ProcessUnitManifest ObtainFromStringCollection(ICollection<string> scollection)
        {
            ProcessUnitManifest types = new ProcessUnitManifest();
            foreach (var item in scollection)
            {
                types.Add(Type.GetType(item));
            }
            return types;
        }
        ///<summary>
        /// Generate a list that contians instances of given types of process units.
        ///</summart>
        public List<IPipedProcessUnit> GetUnitInstances()
        {
            List<IPipedProcessUnit> units = new List<IPipedProcessUnit>();
            foreach (var item in Data)
            {
                units.Add((IPipedProcessUnit)Activator.CreateInstance(item));
            }
            if (units.Count == 0)
            {
                units.Add(new DefaultProcessUnit());
            }
            return units;
        }
        public void Clear()
        {
            Data.Clear();
        }

        public bool Contains(Type item)
        {
            return Data.Contains(item);
        }

        public void CopyTo(Type[] array, int ArrayIndex)
        {
            Data.CopyTo(array, ArrayIndex);
        }

        public IEnumerator<Type> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        public int IndexOf(Type item)
        {
            return Data.IndexOf(item);
        }

        public void Insert(int index, Type item)
        {
            Data.Insert(index, item);
        }

        public bool Remove(Type item)
        {
            return Data.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Data.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }
    }

}