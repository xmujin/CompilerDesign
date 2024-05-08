namespace myapp.Model.Lexer
{
    public class Num : Token
    {
        public readonly int value;
        public string? str = null;
        public Num(int v) : base(Tag.NUM)
        {
            value = v;
        }

        // 错误单元
        public Num(string s) : base(Tag.ERROR)
        {
            str = s;
        }

        // 十六进制和八进制的表示
        public Num(int v, string s) : base(Tag.NUM)
        {
            value = v;
            str = s;
        }


        public override string ToString()
        {
            if(str == null)
                return string.Format("{0}:\t({1, -7}, {2, 3})\n", Line, value, tag);
            else
                return string.Format("{0}:\t({1, -7}, {2, 3})\n", Line, str, tag);
        }
    }
}
