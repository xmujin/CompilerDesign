namespace myapp.Model.Lexer
{
    /// <summary>
    /// word类指保留字，标识符等词素
    /// </summary>
    public class Word : Token
    {

        public string lexeme;

        /// <summary>
        /// 构造word, 对于不确定的word，只有标志符
        /// </summary>
        /// <param name="s">传入的词素的字符串</param>
        /// <param name="t">对应的种别码</param>
        public Word(string s, int t) : base(t)
        {
            lexeme = s;

        }

     



        public static readonly Word
            and   = new Word("&&",    Tag.AND  ), or = new Word("||", Tag.OR),
            eq    = new Word("==",    Tag.EQ   ), ne = new Word("!=", Tag.NE),
            lt    = new Word("<=",    Tag.LT   ), gt = new Word(">=", Tag.GT),
            cmt   = new Word("//",    Tag.CMT  ),  // 行注释
            bcmt  = new Word("/*",    Tag.BCMT ), // 块注释起始符号
            bcmte = new Word("*/",    Tag.BCMTE),
            True  = new Word("true",  Tag.TRUE ),
            inc   = new Word("++",    Tag.TRUE),
            incEq = new Word("+=",    Tag.TRUE),
            temp  = new Word("t",     Tag.TEMP),
            minus = new Word("minus", Tag.TEMP),
            False = new Word("false", Tag.FALSE);



        public override string ToString()
        {
            return lexeme;
        }

    }

    
}
