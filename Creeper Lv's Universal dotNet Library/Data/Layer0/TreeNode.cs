﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Layer0
{
    /// <summary>
    /// Node for TreeStructureData.
    /// </summary>
    public class TreeNode
    {
        /// <summary>
        /// Parent node of current node.
        /// </summary>
        public TreeNode ParentNode;
        /// <summary>
        /// Children of current node.
        /// </summary>
        public List<TreeNode> Children=new List<TreeNode>();
        /// <summary>
        /// Name of this node.
        /// </summary>
        public string Name="";
        /// <summary>
        /// Value of this node.
        /// </summary>
        public string Value="";
        /// <summary>
        /// Container of nodes.
        /// </summary>
        public TreeStructureData Parent;
        /// <summary>
        /// Represents separator to separate name and value. Such as: "Name0|Value1"
        /// </summary>
        /// <returns></returns>
        public virtual char GetSeparator() => '|';
        /// <summary>
        /// Add a node to children.
        /// </summary>
        /// <param name="treeNode"></param>
        public void AddChildren(TreeNode treeNode)
        {
            if (treeNode == this)
            {
                throw new SelfContainException();

            }
            Children.Add(treeNode);
            treeNode.ParentNode = this;
        }
        /// <summary>
        /// Obtain the string representation of current node.
        /// </summary>
        /// <param name="Depth"></param>
        /// <returns></returns>
        public string GenerateItem(int Depth)
        {
            String Prefix = "";
            for (int i = 0; i < Depth; i++)
            {
                Prefix += " ";
            }
            Prefix += $"{GetSeparator()}{Name}{GetSeparator()}{Value}";
            //stringBuilder.Append(GetSeparator());
            //stringBuilder.Append(Name);
            //stringBuilder.Append(GetSeparator());
            //stringBuilder.Append(Value);
            return Prefix;
        }
        /// <summary>
        /// Resolve a node with its depth from a string that generated by GenerateItem(int);
        /// </summary>
        /// <param name="Line"></param>
        /// <returns></returns>
        public static (TreeNode,int) ResolveFromLine(string Line)
        {
            TreeNode treeNode = new TreeNode();
            int DepthIndex = Line.IndexOf(treeNode.GetSeparator());
            string Blanks = Line.Substring(0, DepthIndex);
            int Depth = Blanks.Length;
            var tmp = Line.Substring(Depth + 1);
            string name = tmp.Substring(0, tmp.IndexOf(treeNode.GetSeparator()));
            string value = tmp.Substring(name.Length + 1);
            treeNode.Name = name;
            treeNode.Value = value;
            return (treeNode,Depth);
        }
    }
}
