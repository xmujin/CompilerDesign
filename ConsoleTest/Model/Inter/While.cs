using myapp.Model.Inter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = myapp.Model.Symbols.Type;
namespace myapp.Model.Inter
{
    public class While : Stmt
    {
        Expr expr;
        Stmt stmt;

        public While()
        {
            expr = null;
            stmt = null;
        }

        public void Init(Expr x, Stmt s)
        {
            expr = x;
            stmt = s;
            if (expr.type != Type.Bool)
            {
                expr.Error("boolean required in while");
            }
        }
    }
}
