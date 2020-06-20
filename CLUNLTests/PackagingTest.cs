using CLUNL.Packaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLUNLTests
{
    [TestClass]
    public class PackagingTest
    {
        [TestMethod]
        public void PackTest()
        {
            try
            {
                Directory.CreateDirectory("./PackageTest");
            }
            catch (Exception)
            {
            }
            Dictionary<string, DirectoryInfo> Manifest = new Dictionary<string, DirectoryInfo>();
            try
            {
                Directory.CreateDirectory("./PackageTest/1");
            }
            catch (Exception)
            {
            }
            {
                DirectoryInfo directoryInfo = new DirectoryInfo("./PackageTest/1");
                Manifest.Add("Pkg-1", directoryInfo);
                {
                    try
                    {
                        File.Create("./PackageTest/1/a").Close();
                        File.Create("./PackageTest/1/b").Close();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            try
            {
                Directory.CreateDirectory("./PackageTest/2");
            }
            catch (Exception)
            {
            }
            {
                DirectoryInfo directoryInfo = new DirectoryInfo("./PackageTest/2");
                Manifest.Add("Pkg-2", directoryInfo);
                {
                    try
                    {
                        File.Create("./PackageTest/2/a").Close();
                        File.Create("./PackageTest/2/b").Close();
                        File.Create("./PackageTest/2/c").Close();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            MultiPackagePack.CreatePack(Manifest, new DirectoryInfo(Path.GetTempPath()), new FileInfo("./TestPackage.mpp"));
        }
    }
}
