


using Newtonsoft.Json;
using System;

namespace test
{
    public class Identifier : Node
    {

        public string name { get; set; }

        public string idtype { get; set; }

        public Identifier() : base("Identifier")
        {

        }


    }
    public class Node
    {
        public Node(string type)
        {
            this.type = type;
        }

        public string type { get; set; } = "Node";



    }
    public class Program : Node
    {

        public List<Node> body = new List<Node>();

        public Program() : base("Program")
        {
        }
    }

    public class VariableDeclaration : Node
    {
        public VariableDeclaration() : base("VariableDeclaration")
        {
        }

        public List<VariableDeclarator> declarations { get; set; }

    }

    public class VariableDeclarator : Node
    {


        /// <summary>
        /// 变量的标识符
        /// </summary>
        public Identifier id;


        /// <summary>
        /// 变量的初始化
        /// </summary>
        public Node init;


        public VariableDeclarator() : base("VariableDeclarator")
        {
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class HHH
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.body.Add(new VariableDeclaration { declarations = new List<VariableDeclarator> { new VariableDeclarator { id = new Identifier { name = "sdfsdf" } } } });

            var person = new Person
            {
                Name = "John",
                Age = 30
            };

            string json = JsonConvert.SerializeObject(p);
            File.WriteAllText(@"H:\work\CompilerDesign\test\sb.json", json);
        }
    }
}
