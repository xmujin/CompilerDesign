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



        public VariableDeclaration init;
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
            int truelabel = NewLabel();
            if (test.ToString() == "!=")
            {
                BinaryExpression be = test as BinaryExpression;
                quadruples.Add(new Quadruple("jne", be.left.ToString(), be.right.ToString(), "L" + truelabel));
            }
            quadruples.Add(new Quadruple("j", null, null, "L" + a));
            EmitLabel(quadruples, truelabel);
            // 生成循环体语句
            body.Gen(quadruples, b, a);
            update.Gen(quadruples);
            quadruples.Add(new Quadruple("j", null, null, "L" + begin));





        }
        public ForStatement(VariableDeclaration init, Expression test, Expression update, Statement body) : base("ForStatement")
        {
            this.init = init;
            this.test = test;
            this.update = update;
            this.body = body;



        }
    }
}
