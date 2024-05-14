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
        /// 表达式语句对应的表达式
        /// </summary>
        public Expression expression;
        public ExpressionStatement(Expression expression) : base("ExpressionStatement")
        {
            this.expression = expression;
        }
    }
}
