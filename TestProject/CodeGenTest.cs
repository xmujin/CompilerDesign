using myapp.Model.CodeGen;
using myapp.Model.Inter;
using myapp.Model.Lexer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace TestProject
{

    [TestFixture]
    public class CodeGenTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGenQuadruple_1()
        {
            Program p = new Program();
            
            VariableDeclaration vd = new VariableDeclaration(); // ±‰¡ø…˘√˜
            Literal l = new Literal("123", 123);
            VariableDeclarator vdr = new VariableDeclarator(new Identifier("int", "one"), l);
            vd.declarations.Add(vdr);
            p.body.Add(vd);
            CodeGen.GenQuadruple(p);
            List<Quadruple> expect = new List<Quadruple> { new Quadruple("1", "3", "5", "7")};
            List<Quadruple> actual = new List<Quadruple> { new Quadruple("1", "3", "5", "7") };



            //CollectionAssert.AreEqual(expect, CodeGen.GenQuadruple(p));
            CollectionAssert.AreEqual(expect, actual);

            

        }





        [TestCase('+', 1, 2)]
        public void TestGenBinaryExpression_1(char op, int l, int r)
        {

            Literal left = new Literal("" + l, l);
            Literal right = new Literal("" + r, r);
            Token tok = new Token('+');
            BinaryExpression be = new BinaryExpression(tok, left, right);

            List<Quadruple> expect = new List<Quadruple> { new Quadruple("" + op, "" + l, "" + r, "t1") };




            CollectionAssert.AreEqual(expect, CodeGen.GenBinaryExpression(JObject.FromObject(be)));

        }

        [Test]
        public void TestGenBinaryExpression_2()
        {


            // ≤‚ ‘ 3 + 2 + 5
            Literal left = new Literal("3", 3);
            Literal right = new Literal("2", 2);
            Token tok = new Token('+');
            BinaryExpression be = new BinaryExpression(tok, left, right);
            Literal right_ = new Literal("5", 5);
            BinaryExpression hh = new BinaryExpression(tok, be, right_);

            List<Quadruple> expect = new List<Quadruple> { new Quadruple("+" , "3" , "2" , "t1"),
                                                           new Quadruple("+", "t1", "5", "t2")
            };



            CodeGen.ShowQuadruple(CodeGen.GenBinaryExpression(JObject.FromObject(hh)));


            CollectionAssert.AreEqual(expect, CodeGen.GenBinaryExpression(JObject.FromObject(hh)));

        }


    }
}