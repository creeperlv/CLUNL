using CLUNL.Data.Layer0;
using System;
using System.IO;
using System.Linq;

namespace CLUNL.Data.Serializables.CheckpointSystem
{
    /// <summary>
    /// Built for Game
    /// </summary>
    public class CheckpointSystem
    {

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
        /// Create a checkpoint with random name.
        /// </summary>
        public CheckPoint CreateCheckPoint()
        {
            var guid = Guid.NewGuid();
            var subd = StorageFolder.CreateSubdirectory(guid.ToString());
            return new CheckPoint(subd);
        }
        /// <summary>
        /// Remove a checkpoint with given name.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="throwWhenNotExist"></param>
        public void RemoveACheckPoint(string Name, bool throwWhenNotExist = false)
        {
            var CPD = new DirectoryInfo(Path.Combine(StorageFolder.FullName, Name));
            if (CPD.Exists) CPD.Delete();
            else if (throwWhenNotExist) throw new DirectoryNotFoundException();
        }
        /// <summary>
        /// Get or create a check point with given name.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public CheckPoint GetOrCreateCheckPoint(string Name)
        {
            var CPD = new DirectoryInfo(Path.Combine(StorageFolder.FullName, Name));
            if (CPD.Exists)
            {
                return new CheckPoint(CPD);
            }
            else
            {
                CPD.Create();
            }
            return new CheckPoint(CPD);
        }
        /// <summary>
        /// Enumerate current checkpoints, newest first.
        /// </summary>
        /// <returns></returns>
        public string[] EnumerateCheckpoints()
        {
            var d = StorageFolder.EnumerateDirectories().ToArray();
            FileUtilities.SortDirectoryByDate(ref d, 0, d.Length - 1, false);
            string[] dd = new string[d.Length];
            int i = 0;
            foreach (var item in d)
            {
                dd[i] = item.Name;
                i++;
            }

            return dd;
        }
    }

}
