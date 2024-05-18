using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Utils
{
    // 定义树节点
    public class TreeNode
    {
        public string Value { get; set; }
        public List<TreeNode> Children { get; set; }

        public TreeNode(string value)
        {
            Value = value;
            Children = new List<TreeNode>();
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="s">子节点</param>
        /// <returns>返回子节点的引用</returns>
        public TreeNode AddChild(params string[] childs)
        {
            if (childs.Length == 1)
            {
                TreeNode child = new TreeNode(childs[0]);
                Children.Add(child);
                return child;
            }
            else
            {
                foreach (string str in childs)
                {
                    TreeNode child = new TreeNode(str);
                    Children.Add(child);
                }
                return null;
            }
        }
    }
}
