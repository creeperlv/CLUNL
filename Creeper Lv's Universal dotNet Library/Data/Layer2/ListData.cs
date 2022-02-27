using CLUNL.Data.Layer0;
using CLUNL.DirectedIO;
using CLUNL.Massives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CLUNL.Data.Layer2
{
    /// <summary>
    /// List data that directly write a list to stream.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListData<T> : HoldableObject, ICollection<T>, IList<T>, IEnumerable
    {
        //Stream fileStream;
        BasicKeyValueData BasicKeyValueData;
        List<T> RawData;
        /// <summary>
        /// List data that directly write content to stream.
        /// </summary>
        public ListData()
        {
            RawData = new List<T>();
        }
        /// <summary>
        /// List data that directly write content to stream.
        /// </summary>
        /// <param name="Data">Base data</param>
        public ListData(List<T> Data)
        {
            RawData = Data;
        }
        /// <summary>
        /// Load list data from given stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static ListData<T> LoadFromStream(Stream stream)
        {
            ListData<T> LD = new ListData<T>();
            LD.RawData = new List<T>();
            LD.BasicKeyValueData = new BasicKeyValueData(new StreamWR(stream), '=', true);
            var A = LD.BasicKeyValueData.FindValue("Length");
            if (A != null)
            {
                //Avoid situation that the stream is empty. Such as after LWMS.Core.FileSystem.StorageFolder.CreateFile();
                var D = int.Parse(A);
                for (int i = 0; i < D; i++)
                {
                    LD.RawData.Add(LD.ConvertData(LD.BasicKeyValueData.FindValue(i + "")));
                }
            }
            return LD;
        }
        /// <summary>
        /// Create ListData to given file.
        /// </summary>
        /// <param name="FI"></param>
        /// <returns></returns>
        public static ListData<T> CreateToFile(FileInfo FI)
        {
            ListData<T> D = new ListData<T>();
            D.BasicKeyValueData = BasicKeyValueData.CreateToFile(FI, '=');
            D.RawData = new List<T>();
            return D;
        }
        T ConvertData(string OriginalData)
        {
            var d = Convert.ChangeType(OriginalData, typeof(T));
            return (T)d;
        }
        /// <summary>
        /// Create a ListData with given Capacity.
        /// </summary>
        /// <param name="Capacity"></param>
        public ListData(int Capacity)
        {
            RawData = new List<T>(Capacity);
        }
        /// <summary>
        /// Get an item from given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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
        /// <summary>
        /// The the count of current items.
        /// </summary>
        public int Count => RawData.Count;
        /// <summary>
        /// Save data.
        /// </summary>
        public void Save()
        {
            int Handle = new Random().Next();
            BasicKeyValueData.OnHold(Handle);
            BasicKeyValueData.Clear(Handle: Handle);
            BasicKeyValueData.AddValue("Length", RawData.Count + "", Handle: Handle);
            for (int i = 0; i < RawData.Count; i++)
            {
                BasicKeyValueData.AddValue(i + "", RawData[i].ToString(), false, false, Handle);
            }
            BasicKeyValueData.RemoveOldDuplicatedItems();
            BasicKeyValueData.Flush(Handle);
            BasicKeyValueData.Release(Handle);
        }
        void _Save()
        {
            if (LibraryInfo.GetFlag(FeatureFlags.ListData_AutoSave) == 1)
            {
                Save();
            }
            else
            {
                //Ignore.
            }
        }
        /// <summary>
        /// Determine whether current ListData is read only, it always return false. Real readability depends on using stream.
        /// </summary>
        public bool IsReadOnly => false;
        /// <summary>
        /// Add an item.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            RawData.Add(item);
            _Save();
        }
        /// <summary>
        /// Clear all items.
        /// </summary>
        public void Clear()
        {
            RawData.Clear();
            _Save();
        }
        /// <summary>
        /// Determine whether contains given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return RawData.Contains(item);
        }
        /// <summary>
        /// Copy data to given array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            foreach (var item in RawData)
            {
                array.SetValue(item, index);
                index++;
            }
            //RawData.CopyTo(array, index);
        }
        /// <summary>
        /// Copy data to given array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>

        public void CopyTo(T[] array, int arrayIndex)
        {
            RawData.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Get Enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return RawData.GetEnumerator();
        }
        /// <summary>
        /// Get the index of given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            return RawData.IndexOf(item);
        }
        /// <summary>
        /// Insert given item to given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            RawData.Insert(index, item);
            _Save();
        }
        /// <summary>
        /// Remove given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            var __ = RawData.Remove(item);
            _Save();
            return __;
        }
        /// <summary>
        /// Remove an item from given index.
        /// </summary>
        /// <param name="index"></param>
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
