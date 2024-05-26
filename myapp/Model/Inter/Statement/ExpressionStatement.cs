using myapp.Model.CodeGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class ExpressionStatement : Statement
    {

        /// <summary>
        /// 表达式语句对应的表达式, 即3;   a; a+ 3;等表达式后直接加分号的情况
        /// </summary>
        public Expression expression;
        public ExpressionStatement(Expression expression) : base("ExpressionStatement")
        {
            this.expression = expression;
        }

        /// <summary>
        /// 表达式语句的生成  包括赋值表达式
        /// </summary>
        /// <param name="quadruples"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {
            if(expression is CallExpression ce)
            {
                ce.Gen(quadruples);
            }
            else
            {
                expression.Gen(quadruples, b, a);
            }
            
        }


    }
}
