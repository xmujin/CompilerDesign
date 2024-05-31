using myapp.Model.CodeGen;
using myapp.Model.Symbols;
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
            // 声明并赋值
            if(init != null && depth != 0) // 局部变量的四元式生成
            {
                if(init is  LogicExpression)
                {
                    string ids = id.ToString() + $"_{depth}";
                    int label1 = NewLabel();
                    int label2 = NewLabel();
                    // 初始为0
                    quadruples.Add(new Quadruple("=", "0", null, ids));  // 赋值为0
                    init.trueLabel = label1;
                    init.falseLabel = label2;
                    init.Gen(quadruples, b, a);
                    // 成功
                    EmitLabel(quadruples, label1);
                    // 赋值为1
                    quadruples.Add(new Quadruple("=", "1", null, ids));  // 赋值为1
                    EmitLabel(quadruples, label2);
                }
                else if(init is BinaryExpression be)
                {
                    if(be.op.ToString()  == "!=" ||
                        be.op.ToString() == "==" ||
                        be.op.ToString() == ">" ||
                        be.op.ToString() == ">=" ||
                        be.op.ToString() == "<" ||
                        be.op.ToString() == ">="
                    )
                    {
                        string ids = id.ToString() + $"_{depth}";
                        int label1 = NewLabel();
                        int label2 = NewLabel();
                        // 初始为0
                        quadruples.Add(new Quadruple("=", "0", null, ids));  // 赋值为0
                        init.trueLabel = label1;
                        init.falseLabel = label2;
                        init.Gen(quadruples, b, a);
                        // 成功
                        EmitLabel(quadruples, label1);
                        // 赋值为1
                        quadruples.Add(new Quadruple("=", "1", null, ids));  // 赋值为1
                        EmitLabel(quadruples, label2);
                        
                    }
                    else
                    {
                        quadruples.Add(new Quadruple("=", init.Gen(quadruples), null, id.ToString() + $"_{depth}")); // 其他+ - * / 等初始化

                    }
                }
                else
                {
                    quadruples.Add(new Quadruple("=", init.Gen(quadruples), null, id.ToString() + $"_{depth}"));
                }


                
            }
 
        }

        public VariableDeclarator(Identifier id, Expression init) : base("VariableDeclarator")
        {
            this.id = id;
            this.init = init;
        }
    }
    
}
