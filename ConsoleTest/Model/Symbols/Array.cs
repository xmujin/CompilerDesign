using myapp.Model.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Symbols
{
    public class Array : Type
    {
        public Type of; //数组的元素类型
        public int size = 1; // 元素个数

        public Array(int sz, Type p) : base("[]", Tag.INDEX, sz * p.width)
        {
            size = sz;
            of = p;
        }
    }
}
