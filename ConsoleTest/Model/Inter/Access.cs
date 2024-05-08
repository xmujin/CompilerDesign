using myapp.Model.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = myapp.Model.Symbols.Type;

namespace myapp.Model.Inter
{
    public class Access : Op
    {
        public Id array;
        public Expr index;
        public Access(Id a, Expr i, Type p) : base(new Word("[]", Tag.INDEX), p)
        {
            array = a;
            index = i;
        }

        public override Expr Gen()
        {
            return new Access(array, index.Reduce(), type);
        }

        public override void Jumping(int t, int f)
        {
            EmitJumpings(Reduce().ToString(), t, f);
        }

    }
}
