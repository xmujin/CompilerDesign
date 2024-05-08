using myapp.Model.Inter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = myapp.Model.Symbols.Type;
namespace ConsoleTest.Model.Inter
{
    public class Do : Stmt
    {
        Expr expr;
        Stmt stmt;

        public Do()
        {
            expr = null;
            stmt = null;
        }

        public void Init(Stmt s, Expr x)
        {
            expr = x;
            stmt = s;
            if (expr.type != Type.Bool)
            {
                expr.Error("boolean required in do");
            }
        }
    }
}
