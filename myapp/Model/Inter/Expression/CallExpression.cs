using myapp.Model.CodeGen;
using myapp.Model.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class CallExpression : Expression
    {
        /// <summary>
        /// 被调用的函数
        /// </summary>
        Identifier callee;


        List<Expression> arguments;
        public CallExpression(Identifier callee, List<Expression> arguments) : base("callexpression")
        {
            this.callee = callee;
            this.arguments = arguments;
        }

        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {
            
        }
    }
}
