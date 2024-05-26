using myapp.Model.CodeGen;
using myapp.Model.Lexer;
using myapp.Model.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class CallExpression : Expression
    {
        public int paramcount = 0;
        /// <summary>
        /// 被调用的函数
        /// </summary>
        Identifier callee;


        List<Expression> arguments;
        public CallExpression(Identifier callee, List<Expression> arguments) : base("callexpression")
        {
            this.callee = callee;
            this.arguments = arguments;
        }

        /// <summary>
        /// 函数调用的四元式生成
        /// </summary>
        /// <param name="quadruples"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public override string Gen(List<Quadruple> quadruples)
        {
            foreach(var arg in arguments) 
            {
                if(arg is BinaryExpression)
                {
                    var be = (BinaryExpression)arg;
                    // 将二元表达式规约成临时变量
                    string temp =  be.Gen(quadruples);
                    quadruples.Add(new Quadruple("param", temp, null, "p" + (++paramcount)));
                }
                else
                {
                    if (arg is Identifier) // 如果是标志符，leftRes保存的是变量名
                    {
                        VariableSymbol a = (VariableSymbol)symbolTableManager.Lookup(arg.ToString());

                        string param = a.name + "_" + a.scope;
                        quadruples.Add(new Quadruple("param", param, null, null));
                    }
                    else 
                        quadruples.Add(new Quadruple("param", arg.Gen(quadruples), null, null));
                }
                
            }
            string res = "result";
            quadruples.Add(new Quadruple("call", callee.ToString(), arguments.Count + "", res));
            return res;
        }
    }
}
