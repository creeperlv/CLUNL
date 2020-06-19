using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace CLUNL.Packaging
{
    public class MultiPackagePack
    {
        public static void CreatePack(Dictionary<string,DirectoryInfo>Manifest,DirectoryInfo TempDirectory, FileInfo TargetPackage)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(TempDirectory.FullName, "Package.Manifest"));
            StreamWriter sw = new StreamWriter(fileInfo.OpenWrite());
            foreach (var item in Manifest)
            {
                sw.WriteLine(item.Key + "|" + item.Value.Name);
            }

            sw.Close();

        }
        public static void ExtractPack(params string[] PackageName)
        {

        }
    }
}
