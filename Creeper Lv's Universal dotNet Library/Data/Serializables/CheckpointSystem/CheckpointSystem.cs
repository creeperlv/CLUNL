using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CLUNL.Data.Serializables.CheckpointSystem
{
    /// <summary>
    /// Built for Game
    /// </summary>
    public class CheckpointSystem
    {
        Dictionary<string, ICheckpointData> keyValuePairs = new Dictionary<string, ICheckpointData>();
        /// <summary>
        /// Register a checkpoint data to the system's index;
        /// </summary>
        /// <param name="data"></param>
        public void RegisterCheckPointData(ICheckpointData data)
        {
            keyValuePairs.Add(data.GetName(), data);
        }
        private CheckpointSystem()
        {

        }
        DirectoryInfo StorageFolder;
        /// <summary>
        /// The only working check point to help memory control.
        /// </summary>
        public static CheckpointSystem CurrentCheckpointSystem = new CheckpointSystem();
        /// <summary>
        /// Initilize current system with given folder to store data.
        /// </summary>
        /// <param name="StorageFolder"></param>
        public static void Init(string StorageFolder)
        {
            CurrentCheckpointSystem.StorageFolder = new DirectoryInfo(StorageFolder);
        }
        /// <summary>
        /// Create a checkpoint (snapshot).
        /// </summary>
        public void CreateCheckPoint()
        {
            var guid = Guid.NewGuid();
            var subd = StorageFolder.CreateSubdirectory(guid.ToString());

        }
        /// <summary>
        /// Enumerate current checkpoints, newest first.
        /// </summary>
        /// <returns></returns>
        public string[] EnumerateCheckpoints()
        {
            var d = StorageFolder.EnumerateDirectories().ToArray();
            Sort(ref d, 0, d.Length - 1,false);
            string[] dd = new string[d.Length];
            int i = 0;
            foreach (var item in d)
            {
                dd[i] = item.Name;
                i++;
            }

            return dd;
        }
        /// <summary>
        /// Load a check point with given name.
        /// </summary>
        /// <param name="name"></param>
        public void LoadCheckPoint(string name)
        {

        }
        private static void Sort(ref DirectoryInfo[] Data, int L, int R, bool isAscending = true)
        {
            if (L >= R) return;
            int i = _Sort(ref Data, L, R, isAscending);
            Sort(ref Data, L, i - 1, isAscending);
            Sort(ref Data, i + 1, R, isAscending);
        }

        private static int _Sort(ref DirectoryInfo[] Data, int L, int R, bool isAscending)
        {
            DirectoryInfo pivot = Data[L];
            int __L = L;
            int __R = R;
            if (isAscending)
                while (__L < __R)
                {
                    while (Data[__R].CreationTime >= pivot.CreationTime && __L < __R)
                    {
                        __R--;
                    }
                    Data[__L] = Data[__R];
                    while (Data[__L].CreationTime <= pivot.CreationTime && __L < __R)
                    {
                        __L++;
                    }
                    Data[__R] = Data[__L];
                }
            else while (__L < __R)
                {
                    while (Data[__R].CreationTime <= pivot.CreationTime && __L < __R)
                    {
                        __R--;
                    }
                    Data[__L] = Data[__R];
                    while (Data[__L].CreationTime >= pivot.CreationTime && __L < __R)
                    {
                        __L++;
                    }
                    Data[__R] = Data[__L];
                }
            Data[__L] = pivot;
            return __L;
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
        void Load();
    }
}
