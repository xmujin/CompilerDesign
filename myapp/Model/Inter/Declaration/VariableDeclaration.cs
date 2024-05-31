using myapp.Model.CodeGen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class VariableDeclaration : Node
    {
        public VariableDeclaration() : base("VariableDeclaration")
        {
        }

        [JsonProperty(Order = 2)]
        public List<VariableDeclarator> declarations = new List<VariableDeclarator>();


        /// <summary>
        /// 定义变量并加入到符号表中,当给定了变量名，通过添加作用域信息来获取指定的变量
        /// </summary>
        /// <param name="quadruples"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {
            foreach (var variable in declarations)
            {
                if(depth != 0)
                {
                    symbolTableManager.DefineVariable(variable.id.ToString(), 2, variable.id.idtype, "0", "" + depth);
                }
                else
                {
                    symbolTableManager.DefineVariable(variable.id.ToString(), 2, variable.id.idtype, variable.init.ToString(), "" + depth);
                }
                
                
                variable.Gen(quadruples, b, a); // 生成变量的四元式

            }
        }
      

    }
}
