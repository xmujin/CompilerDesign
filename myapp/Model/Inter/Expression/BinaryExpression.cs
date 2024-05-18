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
    public class BinaryExpression : Expression
    {
        [JsonProperty(Order = 3)]
        public Expression left;

        [JsonProperty(Order = 4)]
        public Expression right;


        public static int count = 0;
        public override string Reduce(List<Quadruple> quadruples)
        {
           
            return null;
        }

        public override string Gen(List<Quadruple> quadruples)
        {
            string leftRes = left.Gen(quadruples);
            string rightRes = right.Gen(quadruples);
            string temp = "t" + (++count);
            Quadruple q = new Quadruple(op.ToString(), leftRes, rightRes, temp);
            quadruples.Add(q);
            return temp;
        }

        public BinaryExpression() : base("BinaryExpression")
        {


        }

        public BinaryExpression(Token op, Expression left, Expression right) : base(op, "BinaryExpression")
        {
            this.left = left;
            this.right = right;
        }

    }
}
