using myapp.Model.Inter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class For : Stmt
    {
        Stmt assign;
        Expr exp;
        Stmt end;
        Stmt block;

        public For(Stmt assign, Expr exp, Stmt end, Stmt block) 
        {
            this.assign = assign;
            this.exp = exp;
            this.end = end;
            this.block = block;
        }
    }
}
