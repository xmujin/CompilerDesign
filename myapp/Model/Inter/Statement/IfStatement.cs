using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using myapp.Model.CodeGen;
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
            Quadruple trueLabel = null;
            int label1 = NewLabel();
            int label2 = NewLabel();

            if (test is LogicExpression _test)
            {
                trueLabel = new Quadruple("j" + _test.op.ToString(), _test.left.ToString(), _test.right.ToString(), "L" + label1);
            }
            Quadruple falseLabel = new Quadruple("j", null, null, "L" + label2);

            quadruples.Add(trueLabel);
            quadruples.Add(falseLabel);
            EmitLabel(quadruples, label1); // if语句开始

            consequent.Gen(quadruples, label1, a);
            
            quadruples.Add(new Quadruple("j", null, null, "L" + a)); // if结束

            EmitLabel(quadruples, label2); // else语句开始
            alternate.Gen(quadruples, label2, a);

            EmitLabel(quadruples, a); // if 语句结束


        }
    }
}
