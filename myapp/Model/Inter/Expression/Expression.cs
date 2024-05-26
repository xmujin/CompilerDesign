using myapp.Model.CodeGen;
using myapp.Model.Lexer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class Expression : Node
    {
        [JsonProperty(Order = 2)]
        public Token op;

        public int trueLabel; // 为真时跳转的标签
        public int falseLabel;
        public static readonly int fall = 333;
        public Expression(string type) : base(type)
        {
        }

        /// <summary>
        /// 生成跳转语句
        /// </summary>
        /// <param name="test">条件</param>
        /// <param name="t"></param>
        /// <param name="f"></param>
        public void EmitJumps(string test, int t, int f)
        {
            if (t != 0 && f != 0)
            {
                // 生成四元式
            }
            else if (t != 0)
            {

            }
            else
            {

            }

        }

        public override string ToString()
        {
            return op.ToString();
        }


        /// <summary>
        /// 生成表达式
        /// </summary>
        /// <param name="quadruples"></param>
        /// <returns></returns>

        public virtual string Gen(List<Quadruple> quadruples)
        {
            return null;
        }

        public virtual string Reduce(List<Quadruple> quadruples)
        {
            return null;
        }




        public Expression(Token op, string type) : base(type) 
        {
            this.op = op;
        }
    }
}
