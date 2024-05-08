using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = myapp.Model.Symbols.Type;
namespace myapp.Model.Inter
{
    /// <summary>
    /// 实现左部为标识符，右部为表达式的赋值语句
    /// </summary>
    public class Set : Stmt
    {
        public Id id;
        public Expr expr;
        public Set(Id i, Expr x)
        {
            id = i;
            expr = x;
            if (Check(id.type, expr.type) == null) Error("type error");
        }

        public Type Check(Type p1, Type p2)
        {
            if (Type.Numeric(p1) && Type.Numeric(p2)) return Type.Max(p1, p2);
            else if (p1 == Type.Bool && p2 == Type.Bool) return p1; // Todo 可能有问题
            else return null;
        }

        public override void Gen(int b, int a)
        {
        }
    }
}
