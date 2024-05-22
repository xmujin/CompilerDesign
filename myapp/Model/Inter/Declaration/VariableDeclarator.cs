using myapp.Model.CodeGen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

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
        public Expression init;


        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {
            if(init != null)
            {
                quadruples.Add(new Quadruple("=", init.Gen(quadruples), null, id.ToString()));
            }
        }

        public VariableDeclarator(Identifier id, Expression init) : base("VariableDeclarator")
        {
            this.id = id;
            this.init = init;
        }
    }
    
}
