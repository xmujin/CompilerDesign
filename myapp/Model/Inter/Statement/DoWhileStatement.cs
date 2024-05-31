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
            //int trueLabel = NewLabel();
            //test.trueLabel = Expression.fall;
            test.trueLabel = begin;


            test.falseLabel = Expression.fall;

            EmitLabel(quadruples, begin); // 发射标签

            body.Gen(quadruples, b, a);
            
            test.Gen(quadruples, b, a);



        }



        public DoWhileStatement(Statement body, Expression test) : base("DoWhileStatement")
        {
            this.body = body;
            this.test = test;
        }
    }
}
