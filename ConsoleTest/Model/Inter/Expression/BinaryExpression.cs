using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myapp.Model.Lexer;
namespace myapp.Model.Inter
{
    public class BinaryExpression : Expression
    {
        public BinaryExpression() : base("BinaryExpression")
        {
        }

        public BinaryExpression(Token op, Node left, Node right) : base(op, left, right, "BinaryExpression")
        {
        }
    }
}
