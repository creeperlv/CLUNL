using CLUNL.Data.Layer0;
using CLUNL.Data.Layer1;
using CLUNL.DirectedIO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using System;
using System.IO;

namespace CLUNLTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PressureTest()
        {
            var a=BasicKeyValueData.CreateToFile(new FileInfo("./TestData.data"));
            for (int i = 0; i < 100000; i++)
            {
                a.AddValue("Key." + i + "", "SampleTest");
            }
            //Microsoft.VisualStudio.TestTools.UnitTesting.Logging.Logger.LogMessage("%s",a.FindValue("Key.2"));
            //a.AddValue("ASDDD","Appended Content");
            a.Flush();
            a.Dispose();
        }
        [TestMethod]
        public void TreeResolveTest()
        {
            string Line = "|AAAA|SSS";
            TreeNode treeNode = new TreeNode();
            int DepthIndex = Line.IndexOf(treeNode.GetSeparator());
            string Blanks = Line.Substring(0, DepthIndex);
            int Depth = Blanks.Length;
            var tmp = Line.Substring(Depth+1);
            string name = tmp.Substring(0,tmp.IndexOf(treeNode.GetSeparator()));
            string value = tmp.Substring(name.Length+1);
            Console.WriteLine(Depth);
            Console.WriteLine(name);
            Console.WriteLine(value);
        }
        [TestMethod]
        public void TreeData()
        {
            TreeStructureData treeStructureData = TreeStructureData.CreateToFile(new FileInfo("./TreeData-Test-2.tree"));
            var a = new TreeNode() { Name = "0001", Value = "AAAB" };
            {
                var b = new TreeNode() {Name= "0002", Value="aaa" };
                a.AddChildren(b);
                {
                    var c = new TreeNode() { Name = "0003", Value = "dfjjkgnsdfjkgdfnklgvdfjgi" };
                    b.AddChildren(c);
                }
                {
                    var c = new TreeNode() { Name = "0004", Value = "dfjjkgnsdfjkgdfnklgvdfjgi" };
                    b.AddChildren(c);
                }
                {
                    var c = new TreeNode() { Name = "0005", Value = "dfjjkgnsdfjkgdfnklgvdfjgi" };
                    b.AddChildren(c);
                }
            }
            treeStructureData.RootNode.AddChildren(a);
            treeStructureData.Serialize();
            Console.WriteLine("Completed.");
        }
        [TestMethod]
        public void KeyValueData()
        {
            {
                var a = BasicKeyValueData.CreateToFile(new FileInfo("./TestData2.data"));
                a.Dispose();
            }
            {
                var a = BasicKeyValueData.LoadFromWR(new FileWR(new FileInfo("./TestData2.data")));
                a.AddValue("AAA", "SDFHSDJKGHSDJKFHSDGJKSHGJKDHJKFSDHFJKSD");
                a.AddValue("AAB", "AAFGMNBSDJKFGBSDJKFSDFJHGHDUIFC");
                a.Flush();
                //Console.WriteLine("AAB Value:" + a.FindValue("AAB"));
                Logger.LogMessage("AAB Value:" + a.FindValue("AAB"));
                Assert.IsTrue(a.ContainsKey("AAB"));
                a.DeleteKey("AAB");
                Assert.IsFalse(a.ContainsKey("AAB"));
                a.Flush();
                Assert.AreEqual(null, a.FindValue("AAB"));
                Logger.LogMessage("AAA Value:" + a.FindValue("AAA"));
                Logger.LogMessage("AAB Value:" + (a.FindValue("AAB") == null ? "NULL" : a.FindValue("AAB")));
            }
            {
                var a = BasicKeyValueData.CreateToFile(new FileInfo("./TestData3.ini"));
                a.Dispose();
            }
            {
                var a = INILikeData.LoadFromStream((new FileInfo("./TestData3.ini")).Open(FileMode.Open));
                a.AddValue("AAA", "SDFHSDJKGHSDJKFHSDGJKSHGJKDHJKFSDHFJKSD");
                a.AddValue("AAB", "AAFGMNBSDJKFGBSDJKFSDFJHGHDUIFC");
                a.Flush();
                //Console.WriteLine("AAB Value:" + a.FindValue("AAB"));
                Logger.LogMessage("AAB Value:" + a.FindValue("AAB"));
                Assert.IsTrue(a.ContainsKey("AAB"));
                a.DeleteKey("AAB");
                Assert.IsFalse(a.ContainsKey("AAB"));
                a.Flush();
                Assert.AreEqual(null, a.FindValue("AAB"));
                Logger.LogMessage("AAA Value:" + a.FindValue("AAA"));
                Logger.LogMessage("AAB Value:" + (a.FindValue("AAB") == null ? "NULL" : a.FindValue("AAB")));
            }
        }
    }
}
