using myapp.Model.CodeGen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    class ForStatement : Statement
    {



        public Statement init;
        /// <summary>
        /// 判断表达式
        /// </summary>
        public Expression test;

        /// <summary>
        /// 更新值用的表达式
        /// </summary>
        public Expression update;

        public Statement body;

        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {





            // 赋值语句生成
            
            init.Gen(quadruples, b, a);





            int begin = NewLabel();
            test.trueLabel = Expression.fall;   // 不需要生成true的跳转
            test.falseLabel = a;

            EmitLabel(quadruples, begin);
            test.Gen(quadruples, b, a);
            // 生成循环体语句
            body.Gen(quadruples, b, a);


            update.Gen(quadruples, b, a); // 生成

            quadruples.Add(new Quadruple("jmp", null, null, "L" + begin));





        }
        public ForStatement(Statement init, Expression test, Expression update, Statement body) : base("ForStatement")
        {
            this.init = init;
            this.test = test;
            this.update = update;
            this.body = body;



        }
    }
}
