using myapp.Model.CodeGen;
using myapp.Model.Inter;
using myapp.Model.Lexer;
using myapp.Model.Parser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace TestProject
{


    [TestFixture]
    public class NewTest
    {

        [SetUp]
        public void Setup()
        {
            
            
        }


        /// <summary>
        /// 简单二元表达式测试
        /// 3+2
        /// </summary>
        [Test]
        public void TestBinaryExpression_1()
        {


            // 测试 3 + 2 + 5     *:42   +:43  -:45  /:47
            Literal left = new Literal("3", 3);
            Literal right = new Literal("2", 2);
            Token tok = new Token('+');
            BinaryExpression be = new BinaryExpression(tok, left, right);
            Literal right_ = new Literal("5", 5);
            BinaryExpression hh = new BinaryExpression(tok, be, right_);


            List<Quadruple> expect = new List<Quadruple> { new Quadruple("+", "3", "2", "t1") };

            List<Quadruple> actual = new List<Quadruple>();
            be.Gen(actual);
            
            CodeGen.ShowQuadruple(actual);

            CollectionAssert.AreEqual(expect, actual);
            BinaryExpression.count = 0;

        }

        /// <summary>
        /// 复杂二元表达式测试
        /// 3+2 + 5
        /// </summary>
        [Test]
        public void TestBinaryExpression_2()
        {


            // 测试 3 + 2 + 5     *:42   +:43  -:45  /:47
            Literal left = new Literal("3", 3);
            Literal right = new Literal("2", 2);
            Token tok = new Token('+');
            BinaryExpression be1 = new BinaryExpression(tok, left, right);
            Literal right_ = new Literal("5", 5);
            BinaryExpression be2 = new BinaryExpression(tok, be1, right_);


            List<Quadruple> expect = new List<Quadruple> { 
                new Quadruple("+", "3", "2", "t1"),
                new Quadruple("+", "t1", "5", "t2"),


            };

            List<Quadruple> actual = new List<Quadruple>();
            be2.Gen(actual);

            CodeGen.ShowQuadruple(actual);

            CollectionAssert.AreEqual(expect, actual);


        }

        /// <summary>
        /// 测试if语句
        /// </summary>
        [Test]
        public void TestIfStatement_1()
        {


            // 测试 3 + 2 + 5     *:42   +:43  -:45  /:47
            Literal left = new Literal("3", 3);
            Literal right = new Literal("2", 2);
            Token tok = new Token('>');
            BinaryExpression be = new BinaryExpression(tok, left, right);
            

            BlockStatement blks = new BlockStatement();
            blks.body = new List<Node> { be};

            IfStatement ifs = new IfStatement(be, blks);

            List<Quadruple> expect = new List<Quadruple> {
                new Quadruple("j>", "3", "2", "L2"),
                new Quadruple("j", null, null, "L3"),
                new Quadruple("label", "L2", null, null),
                new Quadruple("j", null, null, "L1"),
                new Quadruple("label", "L3", null, null),
                new Quadruple("label", "L1", null, null),
                               

            };

            List<Quadruple> actual = new List<Quadruple>();
            int after = ifs.NewLabel();
            ifs.alternate = blks;
            ifs.Gen(actual, 0, after);
            CodeGen.ShowQuadruple(actual);

            CollectionAssert.AreEqual(expect, actual);


        }



    }
}
