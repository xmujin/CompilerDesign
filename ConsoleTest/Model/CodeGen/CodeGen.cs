using myapp.Model.Inter;
using Newtonsoft.Json.Linq;
namespace myapp.Model.CodeGen
{


    public class CodeGen
    {


        Parser.Parser paser;
        int count = 1;
        //List<Quadruple> quadruples = new List<Quadruple>();

        public CodeGen()
        {

        }
        public CodeGen(Parser.Parser paser)
        {
            this.paser = paser;
        }


        public static void ShowQuadruple(List<Quadruple> quadruples)
        {
            foreach (Quadruple q in quadruples)
            {
                Console.WriteLine("{0}, {1}, {2}, {3}", q.op, q.arg1, q.arg2, q.result);
            }

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
        /// 生成二进制表达式的函数
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





    }
}
