using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace CLUNL.Packaging
{
    public class MultiPackagePack
    {
        public static void CreatePack(Dictionary<string,string>Manifest,DirectoryInfo TempDirectory, FileInfo TargetPackage)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(TempDirectory.FullName, "Package.Manifest"));
            foreach (var item in Manifest)
            {

            }
        }
        public static void ExtractPack(params string[] PackageName)
        {

        }
    }
}
