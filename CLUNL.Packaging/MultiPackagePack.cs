using CLUNL.Data.Layer0;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace CLUNL.Packaging
{
    public class MultiPackagePack
    {
        public static void CreatePack(Dictionary<string, DirectoryInfo> Manifest, DirectoryInfo TempDirectory, FileInfo TargetPackage)
        {
            Guid guid = Guid.NewGuid();
            TempDirectory = new DirectoryInfo(Path.Combine(TempDirectory.FullName, guid.ToString()));
            if (!TempDirectory.Exists) TempDirectory.Create();
            FileInfo fileInfo = new FileInfo(Path.Combine(TempDirectory.FullName, "Package.Manifest"));
            StreamWriter sw = new StreamWriter(fileInfo.OpenWrite());
            foreach (var item in Manifest)
            {
                sw.WriteLine(item.Key + "|" + item.Value.Name);
                FileUtilities.CopyFolderRecursively(item.Value.FullName, Path.Combine(TempDirectory.FullName, item.Value.Name));
            }
            sw.Close();
            //Start to Zip.
            if (File.Exists(TargetPackage.FullName)) File.Delete(TargetPackage.FullName);
            ZipFile.CreateFromDirectory(TempDirectory.FullName, TargetPackage.FullName);
            TempDirectory.Delete(true);
        }
        public static void ExtractPack(FileInfo PackageFile, DirectoryInfo TempDirectory, DirectoryInfo Target, params string[] PackageName)
        {
            //var stream=PackageFile.Open(FileMode.Open);
            //ZipArchive zipArchive=new ZipArchive(stream);
            //var Manifest=zipArchive.GetEntry("Package.Manifest");
            //var ManiData=Manifest.Open();
            //StreamReader streamReader=new StreamReader(ManiData);
            //string line=null;
            //while ((line=streamReader.ReadLine())!=null)
            //{
            //    foreach(var item in PackageName){
            //        if(line.StartsWith(item+"|")){
            //            var pkgname=line.Substring((item+"|").Length);
            //            foreach (var entry in zipArchive.Entries)
            //            {
            //                if (entry.FullName.StartsWith(pkgname))
            //                {
            //                    entry.ExtractToFile(Path.Combine(Target.FullName,entry.FullName.Substring(pkgname/)))
            //                }
            //            }


            //        }
            //    }
            //}
            TempDirectory = new DirectoryInfo(Path.Combine(TempDirectory.FullName, Guid.NewGuid().ToString()));
            if (!TempDirectory.Exists) TempDirectory.Create();
            if (Target.Exists == false) Target.Create();
            ZipFile.ExtractToDirectory(PackageFile.FullName, TempDirectory.FullName);
            var manifest = File.ReadAllLines(Path.Combine(TempDirectory.FullName, "Package.Manifest"));
            foreach (var item in manifest)
            {
                foreach (var target in PackageName)
                {
                    if (item.StartsWith(target))
                    {
                        var pkgName = item.Substring(target.Length + 1);
                        FileUtilities.MoveFolderRecursively(Path.Combine(TempDirectory.FullName, pkgName), Target.FullName);
                        //Directory.Move(Path.Combine(TempDirectory.FullName, pkgName), Target.FullName);
                    }
                }
            }
            TempDirectory.Delete(true);

        }
    }
}
