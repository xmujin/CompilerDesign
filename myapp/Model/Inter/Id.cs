using myapp.Model.Lexer;
using Type = myapp.Model.Symbols.Type;

namespace myapp.Model.Inter
{
    public class Id : Expr
    {
        public int offset; //相对地址

        /// <summary>
        /// 标识符
        /// </summary>
        /// <param name="id">传入的标识符的Word单元</param>
        /// <param name="p">标识符类型</param>
        /// <param name="b">所在的存储位置</param>
        public Id(Word id, Type p, int b) : base(id, p)
        {
            offset = b;
        }
    }
}
