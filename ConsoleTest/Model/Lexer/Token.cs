namespace myapp.Model.Lexer
{
    public class Token
    {
        /// <summary>
        /// tag代表种别码
        /// </summary>
        public readonly int tag;

        public int Line { get; set; }

        public Token(int t)
        {
            tag = t;
        }

        public override string ToString()
        {

            return "" + (char)tag;
        }



    }
}
