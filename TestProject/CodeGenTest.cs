using myapp.Model.CodeGen;
using myapp.Model.Inter;
using myapp.Model.Lexer;
using myapp.Model.Parser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace TestProject
{

    [TestFixture]
    public class CodeGenTest
    {
        string fileContent = File.ReadAllText(@"D:\work\CompilerDesign\ConsoleTest\test.txt");
        Lexer lex;
        Parser parser;
        [SetUp]
        public void Setup()
        {
            lex = new Lexer(fileContent);
            //Console.WriteLine(lex.GetTokens());
            parser = new Parser(lex);
            parser.Program();
            string json = JsonConvert.SerializeObject(parser.js);
            //File.WriteAllText(@"D:\work\CompilerDesign\ConsoleTest\sb.json", json);
        }



        /// <summary>
        /// int one = 123;
        /// </summary>
        [Test]
        public void TestGenQuadruple_1()
        {


            CodeGen cg = new CodeGen();
           
            

            List<Quadruple> expect = new List<Quadruple> { new Quadruple("=", "123", null, "t1")};

            List<Quadruple> actual = cg.GenQuadruple(parser.js);

            CodeGen.ShowQuadruple(actual);



            CollectionAssert.AreEqual(expect, actual);

            

        }





        [TestCase('+', 1, 2)]
        public void TestGenBinaryExpression_1(char op, int l, int r)
        {
            CodeGen cg = new CodeGen();
            Literal left = new Literal("" + l, l);
            Literal right = new Literal("" + r, r);
            Token tok = new Token('+');
            BinaryExpression be = new BinaryExpression(tok, left, right);

            List<Quadruple> expect = new List<Quadruple> { new Quadruple("" + op, "" + l, "" + r, "t1") };




            CollectionAssert.AreEqual(expect, cg.GenBinaryExpression(JObject.FromObject(be)));

        }

        /// <summary>
        /// 左表达式，右字面量
        /// </summary>
        [Test]
        public void TestGenBinaryExpression_2()
        {
            CodeGen cg = new CodeGen();
            // 测试 3 + 2 + 5     *:42   +:43  -:45  /:47
            Literal left = new Literal("3", 3);
            Literal right = new Literal("2", 2);
            Token tok = new Token('+');
            BinaryExpression be = new BinaryExpression(tok, left, right);
            Literal right_ = new Literal("5", 5);
            BinaryExpression hh = new BinaryExpression(tok, be, right_);

            List<Quadruple> expect = new List<Quadruple> { new Quadruple("+" , "3" , "2" , "t"+1),
                                                           new Quadruple("+", "t1", "5", "t"+2)
            };


            List<Quadruple> actual = cg.GenBinaryExpression(JObject.FromObject(hh));


            CodeGen.ShowQuadruple(actual);


            CollectionAssert.AreEqual(expect, actual);

        }


        /// <summary>
        /// 测试表达式：3 + 2 - 5 * 6， 左为表达式：3 + 2 ，右为表达式5 * 6。
        /// 左表达式，右表达式
        /// </summary>
        [Test]
        public void TestGenBinaryExpression_3()
        {
            CodeGen cg = new CodeGen();
            // 测试 3 + 2 + 5     *:42   +:43  -:45  /:47
            Literal left1 = new Literal("3", 3);
            Literal right1 = new Literal("2", 2);
            Token tok1 = new Token('+');
            BinaryExpression left = new BinaryExpression(tok1, left1, right1);

            Literal left2 = new Literal("5", 5);
            Literal right2 = new Literal("6", 6);
            Token tok2 = new Token('*');
            BinaryExpression right = new BinaryExpression(tok2, left2, right2);

            BinaryExpression all = new BinaryExpression(new Token('-'), left, right);


            List<Quadruple> expect = new List<Quadruple> { new Quadruple("+" , "3" , "2" , "t1"),
                                                           new Quadruple("*", "5", "6", "t2"),
                                                           new Quadruple("-", "t1", "t2", "t3"),
            };

            List<Quadruple> actual = cg.GenBinaryExpression(JObject.FromObject(all));
            CodeGen.ShowQuadruple(actual);

            CollectionAssert.AreEqual(expect, actual);

        }







    }
}