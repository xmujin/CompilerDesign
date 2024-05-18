using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Utils
{
    public class Dot
    {
        TreeNode root;
        string outPath;
        public StreamWriter writer;

        public Dot(TreeNode root)
        {
            this.root = root;
            this.outPath = @"D:\work\CompilerDesign\ConsoleTest\test.dot";
        }
        public Dot(TreeNode root, string outPath)
        {
            this.root = root;
            this.outPath = outPath;
        }

        /// <summary>
        /// 定义有向边
        /// </summary>
        /// <param name="src">源节点</param>
        /// <param name="target">目标节点</param>
        void Edge(string src, string target)
        {
            writer.WriteLine($"\t{src}->{target};");
        }

        /// <summary>
        /// 定义节点
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="label">标签</param>
        void Node(string value, string label)
        {
            writer.WriteLine($"\t{value} [label=\"{label}\"];");
        }

        void CreateDot(TreeNode root)
        {
            Node("" + root.GetHashCode(), root.Value);
            foreach (var child in root.Children)
            {
                Node("" + child.GetHashCode(), child.Value);
                Edge("" + root.GetHashCode(), "" + child.GetHashCode());
                CreateDot(child);
            }
        }

        void ShowTree(TreeNode root)
        {
            Console.WriteLine(root.Value);
            foreach (var child in root.Children)
            {
                ShowTree(child);
            }
        }

        public void CreateDotFile()
        {
            writer = new StreamWriter(outPath);
            writer.WriteLine("digraph BinaryTree {");
            writer.WriteLine("    node [shape=box, style=filled, fillcolor=lightcyan, fontname=\"Microsoft YaHei\"]");
            CreateDot(root);
            // 写完节点生成树
            writer.WriteLine("}");
            writer.Close();
        }




    }
}
