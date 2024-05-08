using Type = myapp.Model.Symbols.Type;
namespace myapp.Model.Inter
{
    public class Else : Stmt
    {
        Expr expr;
        Stmt stmt1, stmt2;
        public Else(Expr x, Stmt s1, Stmt s2)
        {
            expr = x;
            stmt1 = s1;
            stmt2 = s2;
            if (expr.type != Type.Bool)
            {
                // 报告错误
                expr.Error("");
            }
        }

        public override void Gen(int b, int a)
        {
        }



    }
}
