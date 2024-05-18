using myapp.Model.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Symbols
{
    public class Type : Word
    {

        /// <summary>
        /// 存储的类型所占字节数
        /// </summary>
        public int width;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">关键字</param>
        /// <param name="tag">种别码</param>
        /// <param name="w">所占空间的长度</param>
        public Type(string s, int tag, int w) : base(s, tag)
        {
            width = w;
        }

        public string GetTypeStr()
        {
            return lexeme;
        }

        public static readonly Type
            Int   = new Type("int",   Tag.BASIC, 4),
            Float = new Type("float", Tag.BASIC, 8),
            Char  = new Type("char",  Tag.BASIC, 1),
            Bool  = new Type("bool",  Tag.BASIC, 1),
            Void  = new Type("void",  Tag.BASIC, 0)


            ;

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool Numeric(Type p)
        {
            if(p == Type.Char || p == Type.Int || p == Type.Float)
            {
                return true;
            }
            else return false;
        }

        public static Type Max(Type p1, Type p2)
        {
            if (!Numeric(p1) || !Numeric(p2))
            {
                return null;
            }
            else if (p1 == Type.Float || p2 == Type.Float) return Type.Float;
            else if (p1 == Type.Int || p2 == Type.Int) return Type.Int;
            else return Type.Char;
        }
    }
}
