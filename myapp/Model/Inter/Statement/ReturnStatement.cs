using myapp.Model.CodeGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace myapp.Model.Inter
{
    public class ReturnStatement : Statement
    {
        /// <summary>
        /// 返回的参数
        /// </summary>
        Expression argument;

        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {
            /*if (argument is BinaryExpression be)
            {
                // 将二元表达式规约成临时变量
                string temp = be.Gen(quadruples);
                quadruples.Add(new Quadruple("return", temp, null, null));
            }
            else
            {
                quadruples.Add(new Quadruple("param", arg.Gen(quadruples), null, "p" + (++paramcount)));
            
            }*/
            
            string temp = argument.Gen(quadruples);
            quadruples.Add(new Quadruple("return", temp, null, null));


        }

        public ReturnStatement(Expression argument) : base("ReturnStatement")
        {
            this.argument = argument;


            

        }
    }
}
