using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
