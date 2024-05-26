using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using myapp.Model.CodeGen;
using myapp.Model.Symbols;
using Newtonsoft.Json;
namespace myapp.Model.Inter
{
    /// <summary>
    /// If语句
    /// </summary>
    public class IfStatement : Statement
    {
        /// <summary>
        /// if语句的表达式
        /// </summary>
        [JsonProperty(Order = 2)]
        public Expression test;



        /// <summary>
        /// 表示if语句后的语句，可以是block语句，也可以是单个语句
        /// ，或是表达式语句
        /// </summary>
        [JsonProperty(Order = 3)]
        public Statement consequent;


        public Statement alternate = null;

        public IfStatement(Expression test, Statement consequent) : base("IfStatement")
        {
            this.test = test;
            this.consequent = consequent;
        }



        // b表示if语句之前，a表示之后
        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {

            if(alternate != null)
            {
                int label2 = NewLabel();   // B.false

                if (test is BinaryExpression _test)
                {
                    _test.trueLabel = Expression.fall;
                    _test.falseLabel = label2;

                    _test.Gen(quadruples, b, a);
                    //EmitLabel(quadruples, label1); // if语句开始
                    consequent.Gen(quadruples, b, a);
                    quadruples.Add(new Quadruple("jmp", null, null, "L" + a)); // 跳转到if-else末尾
                    EmitLabel(quadruples, label2); // else语句开始
                    alternate.Gen(quadruples, label2, a);


                }
                else if(test is LogicExpression test1)  // 逻辑表达式
                {
                    test1.trueLabel = Expression.fall;
                    test1.falseLabel = label2;

                    test1.Gen(quadruples, b, a);
                    //EmitLabel(quadruples, label1); // if语句开始
                    consequent.Gen(quadruples, b, a);
                    quadruples.Add(new Quadruple("jmp", null, null, "L" + a)); // 跳转到if-else末尾
                    EmitLabel(quadruples, label2); // else语句开始
                    alternate.Gen(quadruples, label2, a);


                }

            }
            else // 单个if语句
            {
                test.trueLabel = Expression.fall;
                test.falseLabel = a;
                test.Gen(quadruples, b, a);
                consequent.Gen(quadruples, b, a);

                
            }



        }
    }
}
