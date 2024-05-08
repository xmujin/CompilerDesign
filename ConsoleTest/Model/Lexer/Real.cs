using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Lexer
{


    /// <summary>
    /// 浮点数的类
    /// </summary>
    public class Real : Token
    {
        public double value;
        public string? str = null;
        public Real(double value) : base(Tag.REAL)
        {
            this.value = value;
        }

        // 自然对数的表示
        public Real(double value, string? str) : base(Tag.REAL)
        {
            this.value = value;
            this.str = str;
        }

        public override string ToString()
        {
            if (str == null)
                return string.Format("{0}:\t({1, -7}, {2, 3})\n", Line, value, tag);
            else
                return string.Format("{0}:\t({1, -7}, {2, 3})\n", Line, str, tag);
        }
    }
}
