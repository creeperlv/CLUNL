using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLUNL.Data.Layer0
{
    public static class Utilities
    {
        public static void CopyFolderRecursively(string Origin,string Target)
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
    }
}
