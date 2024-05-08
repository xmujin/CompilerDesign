using myapp.Model.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = myapp.Model.Symbols.Type;
namespace myapp.Model.Inter
{
    public class Temp : Expr
    {
        static int count = 0;
        int number = 0;
        public Temp(Type p) : base(Word.temp, p)
        {
            number = ++count;
        }
    }
}
