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
    /// 双目运算符  3 + 2
    /// </summary>
    public class Arith : Op
    {
        public Expr expr1, expr2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tok">表示运算符</param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        public Arith(Token tok, Expr x1, Expr x2) : base(tok, null)
        {
            expr1 = x1;
            expr2 = x2;
            // 类型不一致，则选择类型最大的
            type = Type.Max(expr1.type, expr2.type);
            if (type == null) Error("Type error");
        }

        public override Expr Gen()
        {
            return base.Gen();
        }
    }
}
