using myapp.Model.CodeGen;
using myapp.Model.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace myapp.Model.Inter
{

    /// <summary>
    /// 字面量类
    /// </summary>
    public class Literal : Expression
    {

        /// <summary>
        /// 原始字符串
        /// </summary>
        [JsonProperty(Order = 2)]
        public string raw;

        /// <summary>
        /// 解析后的值
        /// </summary>
        [JsonProperty(Order = 3)]
        public int value;

        public Literal(string raw, int value) : base("Literal")
        {
            this.raw = raw;
            this.value = value;
        }


        public override string ToString()
        {
      
            return raw;
        }

        public override string Gen(List<Quadruple> quadruples)
        {
            return raw;
        }

        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {

            string res = raw;


            if (trueLabel != fall && falseLabel != fall)
            {
                quadruples.Add(new Quadruple("jne", res, "0", "L" + trueLabel));
                quadruples.Add(new Quadruple("jmp", null, null, "L" + falseLabel));

            }
            else if (trueLabel != fall)
            {
                quadruples.Add(new Quadruple("jne", res, "0", "L" + trueLabel));
            }
            else if (falseLabel != fall)
            {
                quadruples.Add(new Quadruple("je", res, "0", "L" + falseLabel));
            }
            else
            {

            }


        }
    }
}
