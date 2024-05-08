using myapp.Model.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = myapp.Model.Symbols.Type;
namespace myapp.Model.Inter
{
    public class Logical : Expr
    {
        public Expr expr1, expr2;
        public Logical(Token tok, Expr x1, Expr x2) : base(tok, null)
        {
            expr1 = x1;
            expr2 = x2;
            type = Check(expr1.type, expr2.type);
            if (type == null) Error("Type error");
        }

        public Type Check(Type p1, Type p2) 
        {
            if (p1 == Type.Bool && p2 == Type.Bool)
            {
                return Type.Bool;
            }
            else return null;
        }

        public override Expr Gen()
        {
            return base.Gen();
        }

    }
}
