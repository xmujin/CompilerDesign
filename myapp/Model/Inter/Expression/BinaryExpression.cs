using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myapp.Model.CodeGen;
using myapp.Model.Lexer;
using myapp.Model.Symbols;
using Newtonsoft.Json;

namespace myapp.Model.Inter
{
    public class BinaryExpression : Expression
    {
        [JsonProperty(Order = 3)]
        public Expression left;

        [JsonProperty(Order = 4)]
        public Expression right;

        public static int count = 0; // 需要清零
        public override string Reduce(List<Quadruple> quadruples)
        {
           
            return null;
        }


        /// <summary>
        /// 该函数用于处理关系表达式的生成，会生成跳转语句
        /// </summary>
        /// <param name="quadruples"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public override void Gen(List<Quadruple> quadruples, int b, int a) // 当为条件判断时进行的跳转
        {


            

            string leftRes = left.Gen(quadruples);
            if (left is Identifier) // 如果是标志符，leftRes保存的是变量名
            {
                VariableSymbol z = (VariableSymbol)symbolTableManager.Lookup(leftRes);
                leftRes = z.name + "_" + z.scope;

            }


            string rightRes = right.Gen(quadruples);
            if (right is Identifier)
            {
                VariableSymbol x = (VariableSymbol)symbolTableManager.Lookup(rightRes);
                rightRes = x.name + "_" + x.scope;

            }
            



            string option = null;
            if(op.ToString() == ">")
            {
                option = "jg";
            }
            else if(op.ToString() == "%")
            {
                option = "jne";
            }
            else if(op.ToString() == "<") 
            {
                option = "jl";
            }
            else if (op.ToString() == "<=")
            {
                option = "jle";
            }
            else if(op.ToString() == ">=")
            {
                option = "jge";
            }
            else if (op.ToString() == "==")
            {
                option = "je";
            }
            else
            {
                option = "jne";
            }

            if (trueLabel != fall && falseLabel !=  fall)    // 用于
            {
                if (op.ToString() == "%")
                {
                    string temp = this.Gen(quadruples);
                    quadruples.Add(new Quadruple(option, temp, "0", "L" + trueLabel));
                    quadruples.Add(new Quadruple("jmp", null, null, "L" + falseLabel));
                    return;
                }


                quadruples.Add(new Quadruple(option, leftRes, rightRes, "L" + trueLabel));
                quadruples.Add(new Quadruple("jmp", null, null, "L" + falseLabel));

            }
            else if(trueLabel != fall)
            {
                if (op.ToString() == "%")
                {
                    string temp = this.Gen(quadruples);
                    quadruples.Add(new Quadruple(option, temp, "0", "L" + trueLabel));
       
                    return;
                }


                quadruples.Add(new Quadruple(option, leftRes, rightRes, "L" + trueLabel));
            }
            else if(falseLabel != fall)
            {
                if(option == "jg")
                {
                    option = "jle";
                }
                else if(option == "jle")
                {
                    option = "jg";
                }
                else if (option == "jl")
                {
                    option = "jge";
                }
                else if (option == "jge")
                {
                    option = "jl";
                }
                else if(option == "je")
                {
                    option = "jne";
                }
                else
                {
                    option = "je";
                }

                if (op.ToString() == "%")
                {
                    string temp = this.Gen(quadruples);
                    quadruples.Add(new Quadruple(option, temp, "0", "L" + falseLabel));

                    return;
                }


                quadruples.Add(new Quadruple(option, leftRes, rightRes, "L" + falseLabel));
            }
            else
            {

            }
            


        }

        public override string Gen(List<Quadruple> quadruples)
        {
            string leftRes = left.Gen(quadruples);
            if(left is Identifier) // 如果是标志符，leftRes保存的是变量名
            {
                VariableSymbol a = (VariableSymbol)symbolTableManager.Lookup(leftRes);
                leftRes = a.name + "_" + a.scope;

            }
            string rightRes = right.Gen(quadruples);
            if (right is Identifier)
            {
                VariableSymbol a = (VariableSymbol)symbolTableManager.Lookup(rightRes);
                rightRes = a.name + "_" + a.scope;

            }


            string temp = "#t" + (++count);
            Quadruple q = new Quadruple(op.ToString(), leftRes, rightRes, temp);
            quadruples.Add(q);
            return temp;
        }

        public BinaryExpression() : base("BinaryExpression")
        {


        }

        public BinaryExpression(Token op, Expression left, Expression right) : base(op, "BinaryExpression")
        {
            this.left = left;
            this.right = right;
        }

    }
}
