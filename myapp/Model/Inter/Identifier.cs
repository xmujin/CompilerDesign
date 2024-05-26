using myapp.Model.CodeGen;
using myapp.Model.Lexer;
using myapp.Model.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace myapp.Model.Inter
{
    public class Identifier : Expression
    {

        /// <summary>
        /// 标识符类型
        /// </summary>
        public string idtype;

        /// <summary>
        /// 标识符字符串
        /// </summary>
        public string name;


        public override string Gen(List<Quadruple> quadruples)
        {
            return name;
        }


        /// <summary>
        /// 用于标识符生成不为0的情况
        /// </summary>
        /// <param name="quadruples"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {

            if (trueLabel != fall && falseLabel != fall)
            {
                quadruples.Add(new Quadruple("jne", name, "0", "L" + trueLabel));
                quadruples.Add(new Quadruple("jmp", null, null, "L" + falseLabel));

            }
            else if (trueLabel != fall)
            {
                quadruples.Add(new Quadruple("jne", name, "0", "L" + trueLabel));
            }
            else if (falseLabel != fall)
            {
                quadruples.Add(new Quadruple("je", name, "0", "L" + falseLabel));
            }
            else
            {

            }



        }




        public Identifier(string idtype, string name) : base(new Word(name, Tag.ID), "Identifier")
        {
            this.idtype = idtype;
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }


    }
}
