using myapp.Model.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = myapp.Model.Symbols.Type;
namespace myapp.Model.Inter
{
    public class Constant : Expr
    {
        public Constant(Token tok, Type p) : base(tok, p)
        {

        }
        public Constant(int i) : base(new Num(i), Type.Int)
        {

        }

        public static readonly Constant
            True = new Constant(Word.True, Type.Bool),
            False = new Constant(Word.False, Type.Bool);

        public override void Jumping(int t, int f)
        {
            
        }




    }
}
