using CLUNL.Data.Layer1;
using CLUNL.DirectedIO;
using CLUNL.Exceptions;
using CLUNL.Massives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CLUNL.Data.Layer0
{
    /// <summary>
    /// Basic Key-Value Data which can be operated easily.
    /// It will look like "Key:Value" by default.
    /// </summary>
    public class BasicKeyValueData : HoldableObject, IDisposable, ICollection<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<string, string>>, IEnumerable
    {
        IBaseWR __;
        private bool isSyncingData = false;
        internal char Separator = ':';
        internal List<string> Raw = new List<string>();
        /// <summary>
        /// Indicates the count of data.
        /// </summary>
        public int Count
        {
            get
            {
                int C = 0;
                foreach (var item in Raw)
                {
                    if (item.StartsWith("#")) continue;
                    if (item.Contains(Separator)) C++;
                }
                return C;
            }
        }
        /// <summary>
        /// Whether the data is read only. (Always return false, it depends on given WR.)
        /// </summary>
        public bool IsReadOnly => false;
        /// <summary>
        /// Return header in front of real data content.
        /// </summary>
        /// <returns></returns>

        public virtual string GetHeader() => @"#=================================================
#=INI-Like Data File                             =
#=Created by CLUNL.Data.Layer0.BasicKeyValueData =
#=================================================";
        internal BasicKeyValueData(Stream stream, char Separator = ':', bool AutoLoad = true)
        {
            this.Separator = Separator;
            __ = new StreamWR(stream);
            if (AutoLoad == true) Load();
        }
        internal BasicKeyValueData(IBaseWR WR, char Separator = ':', bool AutoLoad = true)
        {
            __ = WR;
            this.Separator = Separator;
            if (AutoLoad == true) Load();
        }
        internal void Load()
        {
            string Line;
            Raw.Clear();
            __.Position = 0;
            while ((Line = __.ReadLine()) != null)
            {
                Raw.Add(Line);
            }
        }
        /// <summary>
        /// Close using WR.
        /// </summary>
        public void Dispose()
        {
            __.Dispose();
        }
        /// <summary>
        /// This will force to save data in memory to file.
        /// </summary>
        /// <returns>false - Operation is canceled because one operation is on hold.</returns>
        public bool Flush(int Handle = 0)
        {
            if (HitHandle(Handle) == false)
            {
                return false;
            }
            if (isSyncingData == true)
            {
                return false;
            }
            isSyncingData = true;
            __.Position = 0;
            __.SetLength(0);
            foreach (var item in Raw)
            {
                __.WriteLine(item);
            }
            __.Flush();
            isSyncingData = false;
            return true;
        }
        /// <summary>
        /// This will force to save data in memory to file asynchronously.
        /// </summary>
        /// <returns>false - Operation is canceled because one operation is on hold.</returns>
        public async void FlushAsync()
        {
            if (HitHandle(Handle) == false)
            {
                return;
            }
            isSyncingData = true;
            __.Position = 0;
            __.SetLength(0);
            foreach (var item in Raw)
            {
                await __.WriteLineAsync(item);
            }
            await __.FlushAsync();
            isSyncingData = false;
        }
        /// <summary>
        /// Release WR(If WR works properly).
        /// </summary>
        public void Close()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Clear current data.
        /// </summary>
        /// <param name="AutoSave"></param>
        /// <param name="Handle"></param>
        public void Clear(bool AutoSave = true, int Handle = 0)
        {
            if (HitHandle(Handle) == false)
            {
                return;
            }
            Raw.Clear();
            Raw.Add(GetHeader());
            if (AutoSave == true)
            {
                Flush();
            }
        }
        /// <summary>
        /// Find a value with given key.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string FindValue(string Key)
        {
            //string ___;
            //__.BaseStream.Position = 0;
            //while ((___ = __.ReadLine()) != null)
            //{
            //    if (___.StartsWith("#"))
            //    {
            //        continue;
            //    }
            //    if (___.StartsWith(Key + Separator))
            //    {
            //        return ___.Substring((Key + Separator).Length);

            //    }
            //}
            //return null;
            foreach (var item in Raw)
            {
                if (item.StartsWith(Key + Separator))
                {
                    return item.Substring((Key + Separator).Length);
                }
            }
            return null;
        }
        /// <summary>
        /// Delete a key.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="AutoSave"></param>
        /// <param name="Handle"></param>
        /// <returns>When returning false means this object is currently on hold.</returns>
        public bool? DeleteKey(string Key, bool AutoSave = false, int Handle = 0)
        {
            if (HitHandle(Handle) == false)
            {
                return false;
            }
            if (isSyncingData == true)
            {
                return false;
            }
            for (int i = 0; i < Raw.Count; i++)
            {
                if (Raw[i].StartsWith(Key + Separator))
                {
                    Raw.RemoveAt(i);
                    if (AutoSave == true)
                    {
                        Flush();
                    }
                    return true;
                }
            }
            return null;
            //Key not exist
        }
        /// <summary>
        /// Determines whether contains given key.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public bool ContainsKey(string Key)
        {
            for (int i = 0; i < Raw.Count; i++)
            {
                if (Raw[i].StartsWith(Key + Separator))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Add value to current data.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="AutoSave">Indicates whether the data will be saved after this operation.</param>
        /// <param name="Handle"></param>
        /// <param name="IgnoreDuplicateCheck">Duplicate Check will cause huge performance cost when operating data in a huge scale, recommand ignores it and check it at last.</param>
        /// <returns><p>true: Operate Succeed.</p><br/><p>false: Operation calceld due to other operation is on hold.</p></returns>
        public bool? AddValue(string Key, string Value,bool IgnoreDuplicateCheck=false, bool AutoSave = false, int Handle = 0)
        {
            if (HitHandle(Handle) == false)
            {
                return false;
            }
            if (isSyncingData == true)
            {
                return false;
            }
            if(IgnoreDuplicateCheck==false)
            for (int i = 0; i < Raw.Count; i++)
            {
                if (Raw[i].StartsWith(Key + Separator))
                {
                    Raw[i] = Key + Separator + Value.Replace("\r", "[:R;]").Replace("\n", "[:N;]");
                    if (AutoSave == true)
                    {
                        Flush();
                    }
                    return true;
                }
            }
            Raw.Add(Key + Separator + Value.Replace("\r", "[:R;]").Replace("\n", "[:N;]"));
            if (AutoSave == true)
            {
                Flush();
            }
            return true;
            //____.BaseStream.Position = 0;
            //string ___;
            //StringBuilder stringBuilder = new StringBuilder("");
            //while ((___ = __.ReadLine()) != null)
            //{
            //    if (___.StartsWith("#"))
            //    {
            //        stringBuilder.Append(___);
            //        stringBuilder.Append(Environment.NewLine);
            //        continue;
            //    }
            //    if (___.StartsWith(Key + Separator))
            //    {
            //        continue;
            //    }
            //    stringBuilder.Append(___);
            //    stringBuilder.Append(Environment.NewLine);
            //}
            //stringBuilder.Append($"{Key}={Value}");
            //stringBuilder.Append(Environment.NewLine);
            //____.BaseStream.Seek(0, SeekOrigin.Begin);
            //____.BaseStream.SetLength(0);
            //____.Write(stringBuilder);
            //if (AutoSave == true)
            //{
            //    ____.Flush();
            //}
        }
        /// <summary>
        /// <para>Remove duplicated items that is older. Such as :</para>
        /// <para>KEY1:OLD</para>
        /// <para>KEY1:NEWER</para>
        /// <para>KEY1:NEWEST</para>
        /// <para>Only KEY1:NETEST will remains.</para>
        /// </summary>
        /// <param name="AutoSave"></param>
        /// <param name="Handle"></param>
        /// <returns></returns>
        public bool RemoveOldDuplicatedItems(bool AutoSave = true, int Handle = 0)
        {
            if (HitHandle(Handle) == false)
            {
                return false;
            }
            if (isSyncingData == true)
            {
                return false;
            }
            List<string> Keys=new List<string>();
            for (int i = Raw.Count-1; i >=0 ; i--)
            {
                if (Raw[i].StartsWith("#")) continue;
                if (Raw[i].Contains(Separator))
                {
                    foreach (var item in Keys)
                    {
                        if (Raw[i].StartsWith(item))
                        {
                            Raw.RemoveAt(i);
                        }
                        else
                        {
                            Keys.Add(Raw[i].Substring(0, Raw[i].IndexOf(Separator) + 1));
                        }
                    }
                }
            }
            if (AutoSave == true)
            {
                Flush();
            }
            return true;
        }
        /// <summary>
        /// Create a BasicKeyValueData to a file.
        /// </summary>
        /// <param name="TargetFile"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static BasicKeyValueData CreateToFile(FileInfo TargetFile, char Separator = ':')
        {
            if (!TargetFile.Exists) TargetFile.Create().Close();
            FileWR fileWR = new FileWR(TargetFile);
            fileWR.WriteLine("#=================================================");
            fileWR.WriteLine("#=Key-Value Data File                            =");
            fileWR.WriteLine("#=Created by CLUNL.Data.Layer0.BasicKeyValueData =");
            fileWR.WriteLine("#=================================================");
            fileWR.Flush();
            BasicKeyValueData __＿ = new BasicKeyValueData(fileWR, Separator);
            return __＿;
        }
        /// <summary>
        /// Load a BasicKeyValueData from a WR.
        /// </summary>
        /// <param name="WR"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static BasicKeyValueData LoadFromWR(IBaseWR WR, char Separator = ':')
        {
            BasicKeyValueData _ω___ = new BasicKeyValueData(WR, Separator);
            return _ω___;
        }
        /// <summary>
        /// Load a BasicKeyValueData from a stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static BasicKeyValueData LoadFromStream(Stream stream, char Separator = ':')
        {
            BasicKeyValueData _ω___ = new BasicKeyValueData(stream, Separator);
            return _ω___;
        }

        /// <summary>
        /// Add an item.
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, string> item)
        {
            AddValue(item.Key, item.Value,false,true,0);
        }
        /// <summary>
        /// Clear all data.
        /// </summary>
        public void Clear()
        {
            Clear(true, 0);
        }
        /// <summary>
        /// Determine whether contains given Key-Value combination.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, string> item)
        {
            return ContainsKey(item.Key) ? FindValue(item.Key) == item.Value : false;
        }
        /// <summary>
        /// Copy content to target array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            for (int i = 0; i < this.Count; i++)
            {
                array[arrayIndex + i] = this.ElementAt(i);
            }
        }
        /// <summary>
        /// Remove target item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, string> item)
        {
            return Contains(item) ? (bool)DeleteKey(item.Key) : false;
        }
        /// <summary>
        /// Get Enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var item in Raw)
            {
                if (item.StartsWith("#")) continue;
                if (item.IndexOf(Separator) >= 0)
                {
                    var K= item.Substring(0, item.IndexOf(Separator));
                    var V = item.Substring(item.IndexOf(Separator)+1);
                    yield return new KeyValuePair<string, string>(K,V);

                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in Raw)
            {
                if (item.StartsWith("#")) continue;
                if (item.IndexOf(Separator) >= 0)
                {
                    var K = item.Substring(0, item.IndexOf(Separator));
                    var V = item.Substring(item.IndexOf(Separator) + 1);
                    yield return new KeyValuePair<string, string>(K, V);

                }
            }
        }
    }
}
