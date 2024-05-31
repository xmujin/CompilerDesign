using myapp.Model.CodeGen;
using myapp.Model.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    /// <summary>
    /// i++ , ++i 等自增表达式
    /// </summary>
    class UpdateExpression : Expression
    {

        public bool prefix;  // true 为前缀表达式
        public Expression argument;
        public UpdateExpression(Token op, Expression argument, bool prefix) : base(op ,"UpdateExpression")
        {
            this.argument = argument;
            this.prefix = prefix;
        }


        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {

            

        }








    }
}
