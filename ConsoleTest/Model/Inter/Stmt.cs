using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class Stmt : Node
    {
        public static Stmt Null = new Stmt();
        public static Stmt Enclosing = Stmt.Null;
        public Stmt()
        {
        }


        // 该虚方法用于生成中间代码，暂时不使用
        public virtual void Gen(int b, int a)
        {

        }

    }
}
