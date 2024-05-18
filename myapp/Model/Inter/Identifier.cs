using myapp.Model.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class Identifier : Expression
    {

        /// <summary>
        /// 标识符类型
        /// </summary>
        public string idtype;

        /// <summary>
        /// 标识符字符串
        /// </summary>
        public string name;

        
        public Identifier(string idtype, string name) : base(new Word(name, Tag.ID), "Identifier")
        {
            this.idtype = idtype;
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }


    }
}
