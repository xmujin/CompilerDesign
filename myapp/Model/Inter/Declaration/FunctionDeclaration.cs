using myapp.Model.CodeGen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace myapp.Model.Inter
{
    public class FunctionDeclaration : Node
    {

        /// <summary>
        /// 函数名
        /// </summary>
        [JsonProperty(Order = 2)]
        public Identifier id;


        /// <summary>
        /// 函数参数
        /// </summary>
        [JsonProperty(Order = 3)]
        public List<Identifier> param;

        /// <summary>
        /// 函数体
        /// </summary>
        [JsonProperty(Order = 4)]
        public BlockStatement body;

        
        public FunctionDeclaration() : base("FunctionDeclaration")
        {
        }

        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {
          
            /// 添加函数开始标签
            quadruples.Add(new Quadruple("func", id.ToString(), "begin", null));

            symbolTableManager.DefineFunction(id.ToString(), id.idtype);
            body.Gen(quadruples, b, a);

            /// 添加函数结束标签
            quadruples.Add(new Quadruple("func", id.ToString(), "end", null));

        }


    }
}
