using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{

    /// <summary>
    /// 变量的声明
    /// </summary>
    public class VariableDeclarator : Node
    {


        /// <summary>
        /// 变量的标识符
        /// </summary>
        [JsonProperty(Order = 2)]
        public Identifier id;


        /// <summary>
        /// 变量的初始化
        /// </summary>
        [JsonProperty(Order = 3)]
        public Node init;
        
        
        public VariableDeclarator(Identifier id, Node init) : base("VariableDeclarator")
        {
            this.id = id;
            this.init = init;
        }
    }
    
}
