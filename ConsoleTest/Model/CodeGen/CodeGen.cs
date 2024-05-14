using myapp.Model.Inter;
using Newtonsoft.Json.Linq;
namespace myapp.Model.CodeGen
{


    public class CodeGen
    {


        Parser.Parser paser;

        //List<Quadruple> quadruples = new List<Quadruple>();

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
        public static List<Quadruple> GenQuadruple(Program program)
        {
            List<Quadruple> quadruples = new List<Quadruple>();
            JObject node = JObject.FromObject(program);
            JArray body = (JArray)node["body"];
            Console.WriteLine(node["body"]);
            foreach (JObject bodyNode in body) 
            {
                //变量声明
                if (bodyNode["type"].ToString() == "VariableDeclaration") 
                {
                    JArray vds = (JArray)bodyNode["declarations"];
                    // 遍历variabledeclarator
                    foreach (JObject vdsNode in vds)
                    {
                        JObject init = (JObject)vdsNode["init"];
                        if (init["type"].ToString() == "BinaryExpression")
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
        public static List<Quadruple> GenBinaryExpression(JObject obj)
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
                    quadruples.Add(new Quadruple("" + (char)opch, left, right, "t1"));
                }
                else
                {
                    quadruples.AddRange(GenBinaryExpression((JObject)obj["right"]));
                    quadruples.Add(new Quadruple("" + (char)opch, quadruples.Last<Quadruple>().result, right, "t" + quadruples.Count + 1));
                }
            }
            else
            {
                if (obj["right"]["type"].ToString() == "Literal")
                {
                    right = obj["right"]["value"].ToString();
                    quadruples.AddRange(GenBinaryExpression((JObject)obj["left"]));
                    quadruples.Add(new Quadruple("" + (char)opch, quadruples.Last<Quadruple>().result, right, "t" + (quadruples.Count + 1)));
                }
                else  // 左不是字面量，右也不是字面量
                {



                }
                
            }




            return quadruples;

        }



    }
}
