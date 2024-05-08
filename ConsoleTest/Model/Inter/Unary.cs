using myapp.Model.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = myapp.Model.Symbols.Type;
namespace myapp.Model.Inter
{


    /// <summary>
    /// 处理单目运算符
    /// </summary>
    public class Unary : Op
    {
        public Expr expr;
        public Unary(Token tok, Expr x) : base(tok, null)
        {
            expr = x;
            type = Type.Max(Type.Int, expr.type);
            if (type == null) Error("type error");
        }

        public override Expr Gen()
        {
            return new Unary(op, expr.Reduce());
        }


    }
}
