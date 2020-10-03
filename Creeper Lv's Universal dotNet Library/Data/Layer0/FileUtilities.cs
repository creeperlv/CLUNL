using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLUNL.Data.Layer0
{
    /// <summary>
    /// A tool to help to simplify some file operations.
    /// </summary>
    public static class FileUtilities
    {
        /// <summary>
        /// Copy a folder recursively. (Copy the entire folder.)
        /// </summary>
        /// <param name="Origin"></param>
        /// <param name="Target"></param>
        public static void CopyFolderRecursively(string Origin, string Target)
        {
            if (!Directory.Exists(Origin)) throw new DirectoryNotFoundException();
            if (!Directory.Exists(Target)) Directory.CreateDirectory(Target);
            DirectoryInfo directoryInfo = new DirectoryInfo(Origin);
            DirectoryInfo directoryInfo1 = new DirectoryInfo(Target);
            foreach (var item in directoryInfo.EnumerateFiles())
            {
                item.CopyTo(Path.Combine(directoryInfo1.FullName, item.Name));
            }
            foreach (var item in directoryInfo.EnumerateDirectories())
            {
                CopyFolderRecursively(item.FullName, Path.Combine(directoryInfo1.FullName, item.Name));
            }
        }
        /// <summary>
        /// Move a folder recursively. (Move the entire folder.)
        /// </summary>
        /// <param name="Origin"></param>
        /// <param name="Target"></param>
        public static void MoveFolderRecursively(string Origin, string Target)
        {
            if (!Directory.Exists(Origin)) throw new DirectoryNotFoundException();
            if (!Directory.Exists(Target)) Directory.CreateDirectory(Target);
            DirectoryInfo directoryInfo = new DirectoryInfo(Origin);
            DirectoryInfo directoryInfo1 = new DirectoryInfo(Target);
            foreach (var item in directoryInfo.EnumerateFiles())
            {
                var tgt = Path.Combine(directoryInfo1.FullName, item.Name);
                if (File.Exists(tgt)) File.Delete(tgt);
                item.MoveTo(tgt);
            }
            foreach (var item in directoryInfo.EnumerateDirectories())
            {
                MoveFolderRecursively(item.FullName, Path.Combine(directoryInfo1.FullName, item.Name));
            }
        }
    }
}
