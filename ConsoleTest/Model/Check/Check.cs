using myapp.Model.Symbols;
using myapp.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace myapp.Model.Check
{

    /// <summary>
    /// 语义分析，先生成语法树
    /// </summary>
    public class Check
    {
        const string VdelFdecl = "vdelFdecl";
        //const string GdelFdecl = "gdelFdecl";

        TreeNode root;
        public Check(TreeNode root)
        {
            this.root = root;
        }


        public void CreateEnv()
        {
            

            Stack<TreeNode> stack = new Stack<TreeNode>();
            Env top = new Env();
            stack.Push(root);

            while(stack.Count > 0) 
            {
                TreeNode node = stack.Pop();
                if (node.Value == VdelFdecl)
                {
                    
                }


                for (int i = node.Children.Count - 1; i >= 0; i--) 
                {
                    stack.Push(node.Children[i]);
                }

            }
        }

    }
}
