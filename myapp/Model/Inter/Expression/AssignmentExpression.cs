using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myapp.Model.CodeGen;
using myapp.Model.Lexer;
using myapp.Model.Symbols;
using Newtonsoft.Json;
namespace myapp.Model.Inter
{
    public class AssignmentExpression : Expression
    {

        public Expression left;


        public Expression right;

        public AssignmentExpression() : base("AssignmentExpression")
        {
        }

        public AssignmentExpression(Token op, Expression left, Expression right) : base(op, "AssignmentExpression")
        {
            this.left = left;
            this.right = right;
        }

        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {
            string id = left.ToString();
            if (left is Identifier)
            {
                VariableSymbol sb = (VariableSymbol)symbolTableManager.Lookup(left.ToString());
                id = sb.name + "_" + sb.scope;
            }
            

            if(right is LogicExpression)
            {
                // 如果赋值语句是 a > b (比较), !=, == , &&, , ||  需要单独考虑，因为要对变量赋值为0或1


                int label = NewLabel();
                // 初始为0
                quadruples.Add(new Quadruple("=", "0", null, id));  // 赋值为0

                right.trueLabel = label;
                right.falseLabel = a;
                right.Gen(quadruples, b, a);
                // 成功
                EmitLabel(quadruples, label);
                // 赋值为1
                quadruples.Add(new Quadruple("=", "1", null, id));  // 赋值为1



            }
            else if(right is BinaryExpression && (right.op.ToString() == "!=" ||
                right.op.ToString() == "==" ||
                right.op.ToString() == ">"  ||
                right.op.ToString() == ">=" ||
                right.op.ToString() == "<"  ||
                right.op.ToString() == ">=" 
                ))
            {
                string ids = id.ToString();
                int label1 = NewLabel();
                int label2 = NewLabel();
                // 初始为0
                quadruples.Add(new Quadruple("=", "0", null, ids));  // 赋值为0
                right.trueLabel = label1;
                right.falseLabel = label2;
                right.Gen(quadruples, b, a);
                // 成功
                EmitLabel(quadruples, label1);
                // 赋值为1
                quadruples.Add(new Quadruple("=", "1", null, ids));  // 赋值为1
                EmitLabel(quadruples, label2);
            }
            else
            {

                quadruples.Add(new Quadruple("=", right.Gen(quadruples), null, id.ToString()));
            }
                
        }

    }
}
