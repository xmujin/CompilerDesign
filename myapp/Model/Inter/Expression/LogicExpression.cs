using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myapp.Model.Lexer;
using Newtonsoft.Json;
namespace myapp.Model.Inter
{
    public class LogicExpression : Expression
    {
        public Expression left;
        public Expression right;
        public LogicExpression() : base("LogicExpression")
        {
        }

        public LogicExpression(Token op, Expression left, Expression right) : base(op, "LogicExpression")
        {
            this.left = left;
            this.right = right;
        }
    }
}
