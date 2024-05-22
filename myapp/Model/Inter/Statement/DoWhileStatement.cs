using myapp.Model.CodeGen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    class DoWhileStatement : Statement
    {
 





        public Expression test;
        public Statement body;


        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {
            int begin = NewLabel();
            EmitLabel(quadruples, begin);

            // 生成循环体语句
            body.Gen(quadruples, b, a);

            if (test.ToString() == "!=")
            {
                BinaryExpression be = test as BinaryExpression;
                quadruples.Add(new Quadruple("jne", be.left.ToString(), be.right.ToString(), "L" + begin));
            }


        }



        public DoWhileStatement(Statement body, Expression test) : base("DoWhileStatement")
        {
            this.body = body;
            this.test = test;
        }
    }
}
