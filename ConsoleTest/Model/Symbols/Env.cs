using myapp.Model.Inter;
using myapp.Model.Lexer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Symbols
{
    public class Env
    {
        private Dictionary<string, Node> symbols;
        public Env next;

        /// <summary>
        /// 构造符号表
        /// </summary>
        /// <param name="n">父级符号表</param>
        public Env(Env n) 
        {
            symbols = new Dictionary<string, Node>();
            next = n;
        }

        public Env()
        {
            symbols = new Dictionary<string, Node>();
            next = null;
        }

        public void ShowSymbols()
        {
            Console.WriteLine("符号表");
            int count = 0;
            for (Env e = this; e != null; e = e.next)
            {
                foreach(var node in e.symbols)
                {
                    if(node.Value is VariableDeclarator v)
                    {
                        Console.WriteLine("{0}, {1}", v.id, count);
                    }

                }
                count++;


                



            }

            

        }


        /// <summary>
        /// 放入符号表
        /// </summary>
        /// <param name="w">标识符</param>
        /// <param name="i">标识符对应的类型</param>
        public void AddSymbol(string name, Node symbol)
        {
            symbols[name] = symbol;
        }

        public Node GetSymbol(string name)
        {
            for (Env e = this; e != null; e = e.next)
            {
                Node found = e.symbols[name];
                if (found != null) return found;
            }

            return null;

        }
    }
}
