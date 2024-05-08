using myapp.Model.Lexer;
using Type = myapp.Model.Symbols.Type;
namespace myapp.Model.Inter
{
    public class Op : Expr
    {
        public Op(Token tok, Type p) : base(tok, p) { }
        public override Expr Reduce()
        {
            Expr x = Gen();
            Temp t = new Temp(type);
            Emit("");
            return t;
        }
    }
}
