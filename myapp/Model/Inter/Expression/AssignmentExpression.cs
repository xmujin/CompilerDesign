using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myapp.Model.CodeGen;
using myapp.Model.Lexer;
using Newtonsoft.Json;
namespace myapp.Model.Inter
{
    public class AssignmentExpression : Expression
    {

        public Identifier left;


        public Expression right;

        public AssignmentExpression() : base("AssignmentExpression")
        {
        }

        public AssignmentExpression(Token op, Identifier left, Expression right) : base(op, "AssignmentExpression")
        {
            this.left = left;
            this.right = right;
        }

        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {

            quadruples.Add(new Quadruple("=", right.Gen(quadruples), null, left.ToString()));
        }

    }
}
