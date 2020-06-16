using CLUNL.DirectedIO;
using CLUNL.Massives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLUNL.Data.Layer0
{
    public class TreeStructureData : HoldableObject, IDisposable
    {
        IBaseWR WR;
        public TreeNode RootNode = new TreeNode() { Name="Root",Value="Root"};
        public void Serialize()
        {
            List<string> Content = new List<string>();
            int Depth = 0;
            foreach (var item in RootNode.Children)
            {
                ToContent(item, ref Content, Depth);
            }
            WR.AutoFlush = false;
            WR.Position = 0;
            WR.SetLength(0);
            foreach (var item in Content)
            {
                WR.WriteLine(item);
            }
            WR.Flush();
        }
        void ToContent(TreeNode Node, ref List<string> Content, int Depth)
        {
            Content.Add(Node.GenerateItem(Depth));
            foreach (var item in Node.Children)
            {
                ToContent(item, ref Content, Depth + 1);
            }
        }
        public void Deserialize()
        {
            int LastDepth = 0;
            TreeNode BaseNode = null;
            TreeNode OperatingNode;
            TreeNode LastOperatingNode=null;
            string Line;
            while ((Line = WR.ReadLine()) != null)
            {
                if (Line.TrimStart().StartsWith("#"))
                {

                }
                else
                {
                    var D = TreeNode.ResolveFromLine(Line);
                    OperatingNode = D.Item1;
                    var CurrentDeepth = D.Item2;
                    if (CurrentDeepth == 0)
                    {
                        BaseNode = OperatingNode;
                        RootNode.AddChildren(BaseNode);
                        continue;
                    }
                    if (CurrentDeepth == LastDepth + 2)
                    {
                        BaseNode = LastOperatingNode;
                        BaseNode.AddChildren(OperatingNode);
                        LastDepth++;
                    }
                    else if (CurrentDeepth < LastDepth + 1)
                    {
                        var Delta = LastDepth  - CurrentDeepth;

                        for (int i = 0; i <= Delta; i++)
                        {
                            BaseNode = BaseNode.ParentNode;
                        }
                        BaseNode.AddChildren(OperatingNode);
                        LastDepth -= Delta+1;
                    }
                    else
                    {
                        BaseNode.AddChildren(OperatingNode);
                    }
                    LastOperatingNode = OperatingNode;
                }
            }
        }
        public static TreeStructureData CreateToFile(FileInfo FI)
        {
            TreeStructureData treeStructureData = new TreeStructureData();
            if (!FI.Exists) FI.Create().Close();
            treeStructureData.WR = new FileWR(FI);
            return treeStructureData;
        }
        public static TreeStructureData LoadFromStream(Stream stream)
        {
            TreeStructureData treeStructureData = new TreeStructureData();
            treeStructureData.WR = new StreamWR(stream);
            treeStructureData.Deserialize();
            return treeStructureData;
        }
        public static TreeStructureData LoadFromFile(FileInfo fi)
        {
            TreeStructureData treeStructureData = new TreeStructureData();
            treeStructureData.WR = new FileWR(fi);
            treeStructureData.Deserialize();
            return treeStructureData;
        }

        public void Dispose()
        {
            WR.Dispose();
        }
    }

    [Serializable]
    public class SelfContainException : Exception
    {
        public SelfContainException() : base("Self-Containing is not allowed in this class.") { }
        public SelfContainException(Exception inner) : base("Self-Containing is not allowed in this class.", inner) { }
        protected SelfContainException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
