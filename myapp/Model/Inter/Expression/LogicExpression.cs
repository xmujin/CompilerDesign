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
    public class LogicExpression : Expression
    {
        public Expression left;
        public Expression right;

        public LogicExpression() : base("LogicExpression")
        {
        }


        public override void Gen(List<Quadruple> quadruples, int b, int a) // 只有&& 和 || 是  逻辑表达式
        {

            if(op.ToString() == "||")
            {
                int label1 = NewLabel();
                if(trueLabel != fall)
                {
                    left.trueLabel = trueLabel;
                }
                else
                {
                    left.trueLabel = label1;

                }
                left.falseLabel = Expression.fall;
                right.trueLabel = trueLabel;
                right.falseLabel = falseLabel;
                if(trueLabel != Expression.fall)
                {
                    left.Gen(quadruples, b, a);
                    right.Gen(quadruples, b, a);
                }
                else
                {
                    left.Gen(quadruples, b, a);
                    right.Gen(quadruples, b, a);
                    EmitLabel(quadruples, left.trueLabel);
                }

                
            }
            else
            {
                int label1 = NewLabel();
                if (falseLabel != fall)
                {
                    left.falseLabel = falseLabel;  // 为假直接假
                }
                else
                {
                    left.falseLabel = label1; // 为假直接假，跳转
                }


                left.trueLabel = Expression.fall;   // 当左为真时，不需要跳转
                right.falseLabel = falseLabel;
                right.trueLabel = trueLabel;

                if (falseLabel != Expression.fall)
                {
                    left.Gen(quadruples, b, a);
                    right.Gen(quadruples, b, a);
                }
                else
                {
                    left.Gen(quadruples, b, a);
                    right.Gen(quadruples, b, a);
                    EmitLabel(quadruples, left.falseLabel);
                }

            }
        }

        public LogicExpression(Token op, Expression left, Expression right) : base(op, "LogicExpression")
        {
            this.left = left;
            this.right = right;
        }
    }
}
