using myapp.Model.CodeGen;
using myapp.Model.Lexer;
using myapp.Model.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace myapp.Model.Inter
{
    public class UnaryExpression : Expression
    {


        Expression argument;
        public static int count = 0; // 需要清零
        public UnaryExpression(Token op, Expression argument) : base(op, "UnaryExpression")
        {
            this.argument = argument;

        }

        public override string Gen(List<Quadruple> quadruples)
        {

            string temp = "#t" + (++count);
            string arg = argument.Gen(quadruples);
            if (argument is Identifier)
            {
                VariableSymbol a = (VariableSymbol)symbolTableManager.Lookup(arg);
                arg = a.name + "_" + a.scope;

            }
            Quadruple q = new Quadruple("minus", arg, null, temp);
            quadruples.Add(q);
            return temp;

        }
    }
}
