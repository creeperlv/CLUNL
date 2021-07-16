using CLUNL.Data.Layer0;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CLUNL.Data.Serializables.CheckpointSystem
{
    /// <summary>
    /// A check point, a collection of data.
    /// </summary>
    public class CheckPoint
    {
        DirectoryInfo StorageFolder;
        static JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            NullValueHandling = NullValueHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
        };
        internal CheckPoint(DirectoryInfo di)
        {
            StorageFolder = di;

        }
        /// <summary>
        /// The name of the checkpoint.
        /// </summary>
        public string Name { get => StorageFolder.Name; }
        Dictionary<string, ICheckpointData> ReferenceDatas = new Dictionary<string, ICheckpointData>();
        /// <summary>
        /// Register a checkpoint data to the system's index;
        /// </summary>
        /// <param name="data"></param>
        public void RegisterCheckPointData(ICheckpointData data)
        {
            ReferenceDatas.Add(data.GetName(), data);
        }
        /// <summary>
        /// Create a snapshot of current state.
        /// </summary>
        public string CreateSnapshot()
        {
            Dictionary<string, object> RealData = new Dictionary<string, object>();
            foreach (var item in ReferenceDatas)
            {
                var d = item.Value.Save();
                RealData.Add($"{item.Key}.Count", d.Count);
                for (int i = 0; i < d.Count; i++)
                {

                    RealData.Add($"{item.Key}.{i}", d[i]);
                }
            }
            var Name = Guid.NewGuid().ToString();
            File.WriteAllText(Path.Combine(StorageFolder.FullName, Name), JsonConvert.SerializeObject(RealData, serializerSettings));
            return Name;
        }
        /// <summary>
        /// Load a snapshot with given name of the checkpoint.
        /// </summary>
        public void LoadSnapshot(string Name)
        {
            var path = Path.Combine(StorageFolder.FullName, Name);
            if (File.Exists(path))
            {

                Dictionary<string, object> RealData;
                RealData = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(path), serializerSettings);
                foreach (var item in ReferenceDatas)
                {
                    if (RealData.ContainsKey($"{item.Key}.Count"))
                    {
                        int Count = (int)RealData[$"{item.Key}.Count"];
                        List<Object> d = new List<object>();
                        for (int i = 0; i < Count; i++)
                        {
                            d.Add(RealData[$"{item.Key}.{i}"]);
                        }
                        ReferenceDatas[item.Key].Load(d);
                    }
                }

                //foreach (var item in RealData)
                //{
                //    ReferenceDatas[item.Key].Load(item.Value);
                //}

            }
        }
        /// <summary>
        /// Enumerate the names of snapshots
        /// </summary>
        /// <returns></returns>
        public string[] EnumerateSnapshots()
        {
            var arr = StorageFolder.EnumerateFiles().ToArray();
            FileUtilities.SortFileByDate(ref arr, 0, arr.Length - 1, false);
            string[] result = new string[arr.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = arr[i].Name;
            }
            return result;
        }
    }
    /// <summary>
    /// State Manager
    /// </summary>
    public interface ICheckpointData
    {
        /// <summary>
        /// The unique name of the check point data.
        /// </summary>
        /// <returns></returns>
        string GetName();
        /// <summary>
        /// Save action (CreateCheckPoint)
        /// </summary>
        /// <returns></returns>
        List<object> Save();
        /// <summary>
        /// Load action.
        /// </summary>
        void Load(List<object> data);
    }
}
