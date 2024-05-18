using myapp.Model.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.CodeGen
{
    public class Quadruple
    {
        public string op { get;  set; }
        public string arg1 { get; set; }
        public string arg2 { get; set; }
        public string result { get; set; }
        public Quadruple(string op, string arg1, string arg2, string result)
        {
            this.op = op;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.result = result;

        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Quadruple other = obj as Quadruple;
            return op == other.op && arg1 == other.arg1 && arg2 == other.arg2 && result == other.result;
        }


        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", op, arg1, arg2, result);
        }
    }
}
