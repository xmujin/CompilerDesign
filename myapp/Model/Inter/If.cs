using Type = myapp.Model.Symbols.Type;
namespace myapp.Model.Inter
{
    public class If : Stmt
    {
        public Expr expr;
        public Stmt stmt;
        public If(Expr x, Stmt s)
        {
            expr = x; 
            stmt = s;
            if(expr.type != Type.Bool)
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
