using CLUNL.Data.Layer0;
using CLUNL.DirectedIO;
using CLUNL.Massives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CLUNL.Data.Layer2
{
    public class ListData<T> : HoldableObject, ICollection<T>, IList<T>, IEnumerable
    {
        Stream fileStream;
        BasicKeyValueData BasicKeyValueData;
        List<T> RawData;
        public ListData()
        {
            RawData = new List<T>();
        }
        public ListData(List <T> Data)
        {
            RawData = Data;
        }
        public static ListData<T> LoadFromStream(Stream stream)
        {
            ListData<T> LD = new ListData<T>();
            LD.RawData = new List<T>();
            LD.BasicKeyValueData= new BasicKeyValueData(new StreamWR(stream), '=', true);
            var D=int.Parse(LD.BasicKeyValueData.FindValue("Length"));
            for (int i = 0; i < D; i++)
            {
                LD.RawData.Add(LD.ConvertData(LD.BasicKeyValueData.FindValue(i + "")));
            }
            return LD;
        }
        public static ListData<T> CreateToFile(FileInfo FI)
        {
            ListData<T> D = new ListData<T>();
            D.BasicKeyValueData = BasicKeyValueData.CreateToFile(FI, '=');
            D.RawData = new List<T>();
            return D;
        }
        T ConvertData(string OriginalData)
        {

            var D=RawData.FirstOrDefault();
            var d=Convert.ChangeType(OriginalData, D.GetType());
            return (T)d;
        }
        public ListData(int Capacity)
        {
            RawData = new List<T>(Capacity);
        }
        public T this[int index]
        {
            get => RawData[index]; set
            {
                if (index < RawData.Count && index >= 0)
                {
                    RawData[index] = value;
                    _Save();
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public int Count => RawData.Count;

        public void Save()
        {
            int Handle = new Random().Next();
            BasicKeyValueData.OnHold(Handle);
            BasicKeyValueData.Clear(Handle:Handle);
            BasicKeyValueData.AddValue("Length", RawData.Count+"",Handle:Handle);
            for (int i = 0; i < RawData.Count; i++)
            {
                BasicKeyValueData.AddValue(i + "", RawData[i].ToString(),false,Handle);
            }
            BasicKeyValueData.Flush(Handle);
            BasicKeyValueData.Release(Handle);
        }
        void _Save()
        {
            if (LibraryInfo.GetFlag(FeatureFlags.ListData_AutoSave)==1)
            {
                Save();
            }
            else
            {
                //Ignore.
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            RawData.Add(item);
            _Save();
        }

        public void Clear()
        {
            RawData.Clear();
            _Save();
        }

        public bool Contains(T item)
        {
            return RawData.Contains(item);
        }

        public void CopyTo(Array array, int index)
        {
            foreach (var item in RawData)
            {
                array.SetValue(item, index);
                index++;
            }
            //RawData.CopyTo(array, index);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            RawData.CopyTo(array, arrayIndex);
        }

        public IEnumerator GetEnumerator()
        {
            return RawData.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return RawData.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            RawData.Insert(index, item);
            _Save();
        }

        public bool Remove(T item)
        {
            var __ = RawData.Remove(item);
            _Save();
            return __;
        }

        public void RemoveAt(int index)
        {
            RawData.RemoveAt(index);
            _Save();
        }
        /// <summary>
        /// Not recommended, please only use to read data.
        /// </summary>
        public List<T> Data { get => RawData; }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return RawData.GetEnumerator();
        }

        //IEnumerator<T> IEnumerable<T>.GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
