using myapp.Model.Inter;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Text;
namespace myapp.Model.CodeGen
{


    public class CodeGen
    {


        Parser.Parser paser;
        int count = 1;
        //List<Quadruple> quadruples = new List<Quadruple>();
        int lables = 1;
        public CodeGen()
        {

        }
        public CodeGen(Parser.Parser paser)
        {
            this.paser = paser;
        }

        /// <summary>
        /// 创建新标签
        /// </summary>
        /// <returns></returns>
        public int Newlabel()
        {
            return lables++;
        }

        /// <summary>
        /// 输出标签
        /// </summary>
        /// <param name="i"></param>
        public void Emitlabel(int i)
        {
            Console.WriteLine("L" + i + ":");

        }

        /// <summary>
        /// 输出语句
        /// </summary>
        /// <param name="s"></param>
        public void Emit(string s)
        {
            Console.WriteLine("\t" + s);
        }



        public static string ShowQuadruple(List<Quadruple> quadruples)
        {
            StringBuilder sb = new StringBuilder();
            int count = 1;
            foreach (Quadruple q in quadruples)
            {
                if(q != null)
                {
                    sb.AppendLine(string.Format("{4}:\t{0}, {1}, {2}, {3}", q.op, q.arg1, q.arg2, q.result, count++));
                    //Console.WriteLine("{4}:\t{0}, {1}, {2}, {3}", q.op, q.arg1, q.arg2, q.result, count++);
                }
                
            }


            return sb.ToString();

        }

        /// <summary>
        /// 传入的参数是抽象语法树的根结点
        /// </summary>
        /// <param name="program"></param>
        public List<Quadruple> GenQuadruple(Program program)
        {
            List<Quadruple> quadruples = new List<Quadruple>();
            JObject node = JObject.FromObject(program);
            JArray body = (JArray)node["body"];
            //Console.WriteLine(node["body"]);
            foreach (JObject bodyNode in body) 
            {
                //变量声明
                if (bodyNode["type"].ToString() == "VariableDeclaration") 
                {
                    JArray vds = (JArray)bodyNode["declarations"];
                    // 遍历variabledeclarator
                    foreach (JObject vdsNode in vds)
                    {
                        JObject init = null;
                        if (vdsNode["init"].Type != JTokenType.Null)
                        {
                            init = (JObject)vdsNode["init"];
                        }
                        if (init != null)
                        {
                            if (init["type"].ToString() == "BinaryExpression")
                            {
                                quadruples.AddRange(GenBinaryExpression(init));
                                quadruples.Add(new Quadruple("=", quadruples.Last().result, null, "t" + count++));
                            }
                            else if (init["type"].ToString() == "Literal")
                            {

                                quadruples.Add(new Quadruple("=", init["value"].ToString(), null, "t" + count++));
                            }
                        }
                        


                    }


                }

                if (bodyNode["type"].ToString() == "FunctionDeclaration")
                {
                    JObject blk = (JObject)bodyNode["body"]; // 块语句
                    JArray stmts = (JArray)blk["body"];
                    foreach (JObject stmt in stmts)
                    {
                        if (stmt["type"].ToString() == "IfStatement")
                        {
                            





                        }

                    }



                }

            }

            return quadruples;
        }

        /// <summary>
        /// 生成二元表达式的函数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<Quadruple> GenBinaryExpression(JObject obj)
        {
            List<Quadruple> quadruples = new List<Quadruple>();
            JObject op = (JObject)obj["op"];
            int opch = int.Parse(op["tag"].ToString());
            string left = null, right = null;

            if (obj["left"]["type"].ToString() == "Literal")
            {
                left = obj["left"]["value"].ToString();
                if (obj["right"]["type"].ToString() == "Literal")
                {
                    
                    right = obj["right"]["value"].ToString();
                    quadruples.Add(new Quadruple("" + (char)opch, left, right, "t"+ count++));

                    
                }
                else
                {
                    quadruples.AddRange(GenBinaryExpression((JObject)obj["right"]));
                    quadruples.Add(new Quadruple("" + (char)opch, quadruples.Last<Quadruple>().result, right, "t" + count++));
                }
            }
            else
            {
                if (obj["right"]["type"].ToString() == "Literal")
                {
                    right = obj["right"]["value"].ToString();
                    quadruples.AddRange(GenBinaryExpression((JObject)obj["left"]));
                    quadruples.Add(new Quadruple("" + (char)opch, quadruples.Last<Quadruple>().result, right, "t" + count++));
                }
                else  // 左表达式，右也是表达式
                {

                    quadruples.AddRange(GenBinaryExpression((JObject)obj["left"]));
                    string leftRes = quadruples.Last<Quadruple>().result;
                    quadruples.AddRange(GenBinaryExpression((JObject)obj["right"]));
                    string rightRes = quadruples.Last<Quadruple>().result;
                    quadruples.Add(new Quadruple("" + (char)opch, leftRes, rightRes, "t" + count++));

                }
                
            }

            return quadruples;

        }



        /// <summary>
        /// if语句的四元式生成
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<Quadruple> GenIfStatement(JObject obj)
        {
            List<Quadruple> quadruples = new List<Quadruple>();
            JObject test = (JObject)obj["test"]; // 条件判断
            JObject consequent = (JObject)obj["consequent"];   // if语句块
            JObject alternate = (JObject)obj["alternate"];   // else语句块

            Quadruple trueLabel = null;
            Quadruple falseLabel = null;
            if (test["type"].ToString() == "LogicExpression")     // 生成跳转语句
            {
                int opch = int.Parse(test["op"]["tag"].ToString());
                trueLabel = new Quadruple("j" + (char)opch, test["left"]["value"].ToString(), test["right"]["value"].ToString(), "-1");
                falseLabel = new Quadruple("j", null, null, "-1");
                quadruples.Add(trueLabel);
                quadruples.Add(falseLabel);
                trueLabel.result = "" + (quadruples.Count + 1);
            }

            if (consequent["type"].ToString() == "BlockStatement") // if语句块
            {
                JArray stmts1 = (JArray)consequent["body"];
                foreach(JObject stmt in stmts1) 
                {
                    if (stmt["type"].ToString() == "ExpressionStatement")
                    {
                        quadruples.AddRange(GenBinaryExpression((JObject)stmt["expression"]));
                        //Console.WriteLine("我是sb");

                    }
                    else if (stmt["type"].ToString() == "IfStatement")
                    {
                        List<Quadruple> sb = GenIfStatement((JObject)stmt);
                        foreach(var q in sb)
                        {
                            if(q.op.StartsWith("j"))
                            {
                                q.result = "" + (int.Parse(q.result) + quadruples.Count);
                            }
                        }
                        quadruples.AddRange(sb);
                    }
                    {

                    }

                }

                //if语句结束执行跳转，跳转的位置不确定，取决于if else结束后的位置
                Quadruple next = new Quadruple("j", null, null, "-1");
                quadruples.Add(next);


                // 执行回填操作，回填if不成立时的条件
                falseLabel.result = "" + (quadruples.Count + 1);


                if (alternate["type"].ToString() == "BlockStatement")  // else 语句块
                {
                    JArray stmts = (JArray)alternate["body"];
                    foreach (JObject stmt in stmts)
                    {
                        if (stmt["type"].ToString() == "ExpressionStatement")
                        {
                            quadruples.AddRange(GenBinaryExpression((JObject)stmt["expression"]));
                            //Console.WriteLine("我是你爹");

                        }
                    }
                    // 执行回填操作，确定当if语句块结束后的跳转
                    next.result = "" + (quadruples.Count + 1);
                }

            }
            
            return quadruples;
        }















    }
}
