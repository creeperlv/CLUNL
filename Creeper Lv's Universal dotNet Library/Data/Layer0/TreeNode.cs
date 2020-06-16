using System;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.Layer0
{
    public class TreeNode
    {
        public TreeNode ParentNode;
        public List<TreeNode> Children=new List<TreeNode>();
        public string Name="";
        public string Value="";
        public TreeStructureData Parent;
        public virtual char GetSeparator() => '|';
        public void AddChildren(TreeNode treeNode)
        {
            if (treeNode == this)
            {
                throw new SelfContainException();

            }
            Children.Add(treeNode);
            treeNode.ParentNode = this;
        }
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
