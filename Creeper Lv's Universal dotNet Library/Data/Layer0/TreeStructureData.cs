using CLUNL.DirectedIO;
using CLUNL.Massives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLUNL.Data.Layer0
{
    /// <summary>
    /// A data in tree structure.
    /// </summary>
    public class TreeStructureData : HoldableObject, IDisposable
    {
        IBaseWR WR;
        /// <summary>
        /// Root Node.
        /// </summary>
        public TreeNode RootNode = new TreeNode() { Name = "Root", Value = "Root" };
        /// <summary>
        /// Write nodes in memory to WR.
        /// </summary>
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
        /// <summary>
        /// Read the entire tree from WR.
        /// </summary>
        public void Deserialize()
        {
            int LastDepth = 0;
            TreeNode BaseNode = null;
            TreeNode OperatingNode;
            TreeNode LastOperatingNode = null;
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
                        var Delta = LastDepth - CurrentDeepth;

                        for (int i = 0; i <= Delta; i++)
                        {
                            BaseNode = BaseNode.ParentNode;
                        }
                        BaseNode.AddChildren(OperatingNode);
                        LastDepth -= Delta + 1;
                    }
                    else
                    {
                        BaseNode.AddChildren(OperatingNode);
                    }
                    LastOperatingNode = OperatingNode;
                }
            }
        }
        /// <summary>
        /// Create a tree to given file.
        /// </summary>
        /// <param name="FI"></param>
        /// <returns></returns>
        public static TreeStructureData CreateToFile(FileInfo FI)
        {
            TreeStructureData treeStructureData = new TreeStructureData();
            if (!FI.Exists) FI.Create().Close();
            treeStructureData.WR = new FileWR(FI);
            return treeStructureData;
        }
        /// <summary>
        /// Load a tree from given stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static TreeStructureData LoadFromStream(Stream stream)
        {
            TreeStructureData treeStructureData = new TreeStructureData();
            treeStructureData.WR = new StreamWR(stream);
            treeStructureData.Deserialize();
            return treeStructureData;
        }
        /// <summary>
        /// Load a tree from given file.
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        public static TreeStructureData LoadFromFile(FileInfo fi)
        {
            TreeStructureData treeStructureData = new TreeStructureData();
            treeStructureData.WR = new FileWR(fi);
            treeStructureData.Deserialize();
            return treeStructureData;
        }
        /// <summary>
        /// Close using WR.
        /// </summary>
        public void Dispose()
        {
            WR.Dispose();
        }
    }
    /// <summary>
    /// Throw when system detected trying to add a node to its children.
    /// </summary>

    [Serializable]
    public class SelfContainException : Exception
    {
        /// <summary>
        /// Throw when system detected trying to add a node to its children.
        /// </summary>
        public SelfContainException() : base("Self-Containing is not allowed in this class.") { }
    }
}
