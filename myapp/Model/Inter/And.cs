using myapp.Model.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class And : Logical
    {
        public And(Token tok, Expr x1, Expr x2) : base(tok, x1, x2)
        {

        }

        public override void Jumping(int t, int f)
        {
            base.Jumping(t, f);
        }
    }
}
