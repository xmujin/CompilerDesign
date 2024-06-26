﻿using myapp.Model.CodeGen;
using myapp.Model.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    int begin = NewLabel();
                    int end = NewLabel();
                    EmitLabel(quadruples, "varDecl_begin");
                    variableDeclaration.Gen(quadruples, begin, end);
                    EmitLabel(quadruples, "varDecl_end");

                    //symbolTable.PrintCurrentScope();


                }
                else if(node is FunctionDeclaration functionDeclaration)
                {
                    

                    int begin = NewLabel();
                    int end = NewLabel();
                    EmitLabel(quadruples, "function_begin");
                    functionDeclaration.Gen(quadruples, begin, end);
                    EmitLabel(quadruples, "function_end");

                }
            
            }

            //symbolTable.ExitScope();
        }
    
           
    }


    
}

