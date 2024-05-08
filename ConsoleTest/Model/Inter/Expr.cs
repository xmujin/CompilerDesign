using myapp.Model.Lexer;
using Type = myapp.Model.Symbols.Type;

namespace myapp.Model.Inter
{
    public class Expr : Node
    {
        public Token op;
        public Type type;
        public Expr(Token op, Type type)
        {
            this.op = op;
            this.type = type;
        }
        public virtual Expr Gen() { return this; }
        public virtual Expr Reduce() { return this; }

        public virtual void Jumping(int t, int f)
        {

        }

        public virtual void EmitJumpings(string test, int t, int f)
        {

        }
    }
}
