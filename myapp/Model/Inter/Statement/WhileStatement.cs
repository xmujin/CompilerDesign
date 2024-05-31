using myapp.Model.CodeGen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    class WhileStatement : Statement
    {

        /// <summary>
        /// while语句的表达式
        /// </summary>
        [JsonProperty(Order = 2)]
        public Expression test;


        /// <summary>
        /// 表示while语句后的语句，可以是block语句，也可以是单个语句
        /// ，或是表达式语句
        /// </summary>
        [JsonProperty(Order = 3)]
        public Statement body;



        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {
            int begin = NewLabel();
            test.trueLabel = Expression.fall;
            test.falseLabel = a;

            EmitLabel(quadruples, begin); // 发射标签
            test.Gen(quadruples, b, a);
            // 生成循环体语句
            body.Gen(quadruples, b, a);
            quadruples.Add(new Quadruple("jmp", null, null, "L" + begin));

        }


        public WhileStatement(Expression test, Statement body) : base("WhileStatement")
        {
            this.test = test;
            this.body = body;
        }
    }
}
