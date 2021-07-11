using System.Collections.Generic;
using System.IO;

namespace CLUNL.Data.Serializables.CheckpointSystem
{
    /// <summary>
    /// A check point, a collection of data.
    /// </summary>
    public class CheckPoint
    {
        DirectoryInfo StorageFolder;
        internal CheckPoint(DirectoryInfo di)
        {
            StorageFolder = di;

        }
        /// <summary>
        /// The name of the checkpoint.
        /// </summary>
        public string Name { get => StorageFolder.Name; }
        Dictionary<string, ICheckpointData> keyValuePairs = new Dictionary<string, ICheckpointData>();
        /// <summary>
        /// Register a checkpoint data to the system's index;
        /// </summary>
        /// <param name="data"></param>
        public void RegisterCheckPointData(ICheckpointData data)
        {
            keyValuePairs.Add(data.GetName(), data);
        }
        /// <summary>
        /// Create a snapshot of current state.
        /// </summary>
        public void CreateSnapshot()
        {

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
        byte[] Save();
        /// <summary>
        /// Load action.
        /// </summary>
        void Load();
    }
}
