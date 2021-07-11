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

        /// <summary>
        /// Sort a directory array by date.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="L">0</param>
        /// <param name="R">Data.Length-1</param>
        /// <param name="isAscending"></param>
        public static void SortDirectoryByDate(ref DirectoryInfo[] Data, int L, int R, bool isAscending = true)
        {
            if (L >= R) return;
            int i = _Sort(ref Data, L, R, isAscending);
            SortDirectoryByDate(ref Data, L, i - 1, isAscending);
            SortDirectoryByDate(ref Data, i + 1, R, isAscending);
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
}
