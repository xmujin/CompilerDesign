using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class Seq : Stmt
    {
        public Stmt stmt1, stmt2;

        public Node node1, node2;
        /// <summary>
        /// 实现一个语句序列
        /// </summary>
        /// <param name="stmt1"></param>
        /// <param name="stmt2"></param>
        public Seq(Stmt stmt1, Stmt stmt2)
        {
            this.stmt1 = stmt1;
            this.stmt2 = stmt2;
        }

        /// <summary>
        /// 实现一个声明节点序列
        /// </summary>
        /// <param name="stmt1"></param>
        /// <param name="stmt2"></param>
        public Seq(Node node1, Node node2)
        {
            this.node1 = node1;
            this.node2 = node2;
        }

        public override void Gen(int b, int a)
        {
            
        }


    }
}
