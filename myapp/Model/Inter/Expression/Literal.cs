using myapp.Model.CodeGen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
