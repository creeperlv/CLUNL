using CLUNL.Data.Layer0;
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
            Guid guid = Guid.NewGuid();
            TempDirectory = new DirectoryInfo(Path.Combine(TempDirectory.FullName, guid.ToString()));
            if(!TempDirectory.Exists)TempDirectory.Create();
            FileInfo fileInfo = new FileInfo(Path.Combine(TempDirectory.FullName, "Package.Manifest"));
            StreamWriter sw = new StreamWriter(fileInfo.OpenWrite());
            foreach (var item in Manifest)
            {
                sw.WriteLine(item.Key + "|" + item.Value.Name);
                Utilities.CopyFolderRecursively(item.Value.FullName, Path.Combine(TempDirectory.FullName, item.Value.Name));
            }
            sw.Close();
            //Start to Zip.
            ZipFile.CreateFromDirectory(TempDirectory.FullName, TargetPackage.FullName);
            TempDirectory.Delete(true);
        }
        public static void ExtractPack(params string[] PackageName)
        {

        }
    }
}
