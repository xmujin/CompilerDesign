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
    public class Program : Node
    {
        


        [JsonProperty(Order = 2)]
        public List<Node> body = new List<Node>();
        public Program() : base("Program")
        {
        }

        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {

            
            foreach(var node in body) 
            {

                
                if(node is VariableDeclaration variableDeclaration)
                {
                    variableDeclaration.Gen(quadruples, b, a);
                }
                else if(node is FunctionDeclaration functionDeclaration)
                {
                    
                    functionDeclaration.Gen(quadruples, b, a);
                }
            
            }
        }
    
           
    }


    
}

