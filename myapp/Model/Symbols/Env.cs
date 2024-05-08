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
        Hashtable table;
        protected Env prev;

        /// <summary>
        /// 构造符号表
        /// </summary>
        /// <param name="n">父级符号表</param>
        public Env(Env n) 
        {
            table = new Hashtable();
            prev = n;
        }

        public Env()
        {
            table = new Hashtable();
            prev = null;
        }

        /// <summary>
        /// 放入符号表
        /// </summary>
        /// <param name="w">标识符</param>
        /// <param name="i">标识符对应的类型</param>
        public void Put(Token w, Id i)
        {
            table.Add(w, i);
        }

        public Id Get(Token w)
        {
            for (Env e = this; e != null; e = e.prev)
            {
                Id found = (Id)(e.table[w]);
                if (found != null) return found;
            }

            return null;
            
        }
    }
}
