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
            TypeNameHandling = TypeNameHandling.All
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
        public void CreateSnapshot()
        {
            Dictionary<string, List<object>> RealData = new Dictionary<string, List<object>>();
            foreach (var item in ReferenceDatas)
            {
                RealData.Add(item.Key, item.Value.Save());
            }
            var Name = Guid.NewGuid().ToString() + ".json";
            File.WriteAllText(Path.Combine(StorageFolder.FullName, Name), JsonConvert.SerializeObject(RealData, serializerSettings));
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
