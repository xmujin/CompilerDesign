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

        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {
            foreach (var variable in declarations)
            {
                symbolTable.AddSymbol(variable.id.ToString(), variable);
            }
        }
      

    }
}
