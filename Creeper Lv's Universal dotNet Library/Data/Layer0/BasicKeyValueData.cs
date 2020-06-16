using CLUNL.Data.Layer1;
using CLUNL.DirectedIO;
using CLUNL.Exceptions;
using CLUNL.Massives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CLUNL.Data.Layer0
{
    public class BasicKeyValueData : HoldableObject,IDisposable
    {
        IBaseWR __;
        private bool isSyncingData = false;
        internal char Separator = ':';
        internal List<string> Raw = new List<string>();
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
        internal BasicKeyValueData(IBaseWR WR,char Separator = ':',bool AutoLoad = true)
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
        public void Dispose()
        {
            __.Dispose();
        }
        /// <summary>
        /// This will force to save data in memory to file.
        /// </summary>
        /// <returns>false - Operation is canceled because one operation is on hold.</returns>
        public bool Flush(int Handle=0)
        {
            if (HitHandle(Handle)==false)
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
        public void Close()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }
        public void Clear(bool AutoSave=true,int Handle=0)
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
        public string FindValue(String Key)
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
        public bool? DeleteKey(string Key, bool AutoSave = false,int Handle=0)
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
        /// <returns><p>true: Operate Succeed.</p><br/><p>false: Operation calceld due to other operation is on hold.</p></returns>
        public bool? AddValue(string Key, string Value, bool AutoSave = false,int Handle=0)
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
        public static BasicKeyValueData LoadFromWR(IBaseWR WR,char Separator=':')
        {
            BasicKeyValueData _ω___ = new BasicKeyValueData(WR, Separator);
            return _ω___;
        }
        public static BasicKeyValueData LoadFromStream(Stream stream, char Separator = ':')
        {
            BasicKeyValueData _ω___ = new BasicKeyValueData(stream, Separator);
            return _ω___;
        }
    }
}
