#define AUTO


using System.Collections;
using System.IO;
using System.Text;
using myapp.Model.Lexer;
using Type = myapp.Model.Symbols.Type;
namespace myapp.Model.Lexer
{

    public partial class LexerScanner
    {
        public StringBuilder sb = new StringBuilder();
        public StringBuilder ErrorString = new StringBuilder();
        //public Hashtable tokens = new Hashtable();
        public List<Token> tokens = new List<Token>();
        public List<string> tokensstr = new List<string>();
        // 这里设置保留字
        Hashtable words = new Hashtable() {
            { "if" ,  new Word("if",Tag.IF)},
            { "else" ,new Word("else",Tag.ELSE)},
            { "while" ,  new Word("while",Tag.WHILE)},
            { "do" ,  new Word("do",Tag.DO)},
            { "break" ,  new Word("break",Tag.BREAK)},
            { "for" ,  new Word("for",Tag.FOR)},
            { Word.True.lexeme ,  Word.True},
            { Word.False.lexeme ,  Word.False},
            { Type.Int.lexeme ,     Type.Int},
            { Type.Bool.lexeme ,  Type.Bool},
            { Type.Float.lexeme ,  Type.Float},
            { Type.Char.lexeme ,  Type.Char},
            { Type.Void.lexeme ,  Type.Void}
        };

      

        void ShowNum()
        {
            sb.Append(string.Format("{0}:\t({1, -7}, {2, 3})\n", yyline, yytext, Tag.NUM));
            //tokens.Add(new Num(int.Parse(yytext)), Tag.NUM);
            tokens.Add(new Num(int.Parse(yytext)));
            tokensstr.Add(yytext); 
        }

        void ShowId()
        {
            sb.Append(string.Format("{0}:\t({1, -7}, {2, 3})\n", yyline, yytext, Tag.ID));
            //Console.WriteLine(yytext);
            Word w = (Word)words[yytext];
            if(w != null)
            {
                tokens.Add(w);
                tokensstr.Add(yytext);
            }
            else
            {
                w = new Word(yytext, Tag.ID);
                words.Add(w.lexeme, w);
                tokens.Add(w);
                tokensstr.Add(yytext);

            }
        }

        void ShowOperator()
        {
            sb.Append(string.Format("{0}:\t({1, -7}, {2, 3})\n", yyline, yytext, (int)yytext[0]));
            //tokens.Add(new Token((int)yytext[0]), (int)yytext[0]);
            tokens.Add(new Token((int)yytext[0]));
            tokensstr.Add(yytext);
        }

        void ShowBoundary()
        {
            sb.Append(string.Format("{0}:\t({1, -7}, {2, 3})\n", yyline, yytext, (int)yytext[0]));
            tokens.Add(new Token((int)yytext[0]));
            tokensstr.Add(yytext);
        }


        void ShowDoubleOpet()
        {
            // ([!*+-/<=>]=)|&&|\|\||\+\+
            int tag = 0;
            switch(yytext) 
            {
                case "!=": tag = Tag.NE; break;
                case "*=": tag = Tag.MULEQ; break;
                case "+=": tag = Tag.INCEQ; break;
                case "-=": tag = Tag.SUBEQ; break;
                case "/=": tag = Tag.DIVEQ; break;
                case "<=": tag = Tag.LT; break;
                case "==": tag = Tag.EQ; break;
                case ">=": tag = Tag.GT; break;
                case "&&": tag = Tag.AND; break;
                case "++": tag = Tag.INC; break;
                case "||": tag = Tag.OR; break;
            }

            sb.Append(string.Format("{0}:\t({1, -7}, {2, 3})\n", yyline, yytext, tag));
            tokens.Add(new Word(yytext, tag));
            tokensstr.Add(yytext);


        }

        void ReportError()
        {
            ErrorString.Append(string.Format("在第{0}行检测到非法字符: {1}\n", yyline, yytext));
        }
    }

    public class Lexer
    {


#if AUTO

        private LexerScanner scanner;
        private string? SourceCode;
        public int Index { get; set; }

        

#pragma warning disable 8604
        public Lexer(string? SourceCode) 
        {
            this.SourceCode = SourceCode;
            byte[] inputBuffer = System.Text.Encoding.UTF8.GetBytes(SourceCode);
            MemoryStream stream = new MemoryStream(inputBuffer);
            scanner = new LexerScanner(stream);
            scanner.yylex();
            Index = 0;


        }
        public Token Scan()
        {
            if (Index < scanner.tokens.Count)
                return scanner.tokens[Index++];
            else return null;
        }
#pragma warning restore 8604

        /// <summary>
        /// 获取Tokens序列及所在的行
        /// </summary>
        /// <returns></returns>
        public string GetTokens()
        {
            return scanner.sb.ToString();
        }

        public string GetErrors()
        {
            return scanner.ErrorString.ToString();
        }

        public string GetTokenList()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < scanner.tokensstr.Count; i++)
            {
                if (i != scanner.tokensstr.Count - 1)
                    sb.Append(scanner.tokensstr[i] + " ");
                else
                    sb.Append(scanner.tokensstr[i]);
            }

            return sb.ToString();
        }




#else

        public int line = 1;
        
        private StringReader _StringReader;
        private string? SourceCode;

        public StringBuilder ErrorString = new StringBuilder();

        char peek = ' ';
        int span = 0;

        /// <summary>
        /// 元素内容为键值对，键代表单词的字符串，值代表该单词对应的word
        /// </summary>
        Hashtable words = new Hashtable();


        List<Token> pair = new List<Token>();

        

        //List<Token>? tokens = new List<Token>();

        public Lexer(string? SourceCode)
        {
            this.SourceCode = SourceCode;
            _StringReader = new StringReader(SourceCode);
            
            Reserve(new Word("if", Tag.IF));
            Reserve(new Word("else", Tag.ELSE));
            Reserve(new Word("do", Tag.DO));
            Reserve(new Word("while", Tag.WHILE));
            Reserve(new Word("break", Tag.BREAK));

        }

        /// <summary>
        /// 用于存储默认的保留字
        /// </summary>
        void Reserve(Word word)
        {
            words.Add(word.lexeme, word);
        }

        void readch()
        {
            peek = (char)_StringReader.Read();
        }

        bool readch(char c)
        {
            readch();
            if (peek != c) return false;
            peek = ' '; // 设置空格会使得下次Scan会被跳过
            return true;
        }


        Word ScanWord(char ch)
        {
            int state = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(ch);
            while (state != 2)
            {
                switch (state)
                {
                    case 0: state = 1; break;
                    case 1:
                        char get = (char)_StringReader.Peek();
                        if (!char.IsLetterOrDigit(get))
                        {
                            state = 2;
                        }
                        else
                        {
                            ch = (char)_StringReader.Read();
                            sb.Append(ch);
                        }
                        break;
                }
            }
            string s = sb.ToString();
            //Word w = (Word)words[s];
            /*if (w != null)
            {
                w.Line = line;
                return w;
            } */
            Word w = new Word(s, Tag.ID);
            w.Line = line;
            //words.Add(s, w);
            return w;
        }


        bool IsBoundary(char ch)
        {
            return ch == ',' || ch == ';' || ch == '{' || ch == '}' || ch == ' ';
        }

        bool IsOperater(char ch)
        {
            char[] op = new char[]{'+', '-', '*', '/', '=', '&', '|', '!', '<', '>', '(', ')', '[', ']'};
            foreach(char c in op) 
            {
                if(c == ch)
                    return true;
            }
            return false;
        }


        Token? ScanNum(char ch)
        {
            int state = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(ch);
            while (state != 2 && state != 4 && state != 7)
            {
                switch (state)
                {
                    case 0:
                        if (ch != '0') state = 1; else { state = 3; }
                        break;
                    case 1:
                        {
                            char get = (char)_StringReader.Peek(); ;
                            if(_StringReader.Peek() == -1)
                            {
                                state = 2;
                                break;
                            }
                            

                            if (char.IsDigit(get))
                            {
                                _StringReader.Read();
                                //v = v * 10 + get - '0';
                                sb.Append(get);
                            }
                            else if(get == '.')
                            {
                                // 以非0开头的浮点数识别
                                state = 8;
                                _StringReader.Read();
                                sb.Append(get);
                            }
                            else if(get == 'e')
                            {
                                state = 9;
                                _StringReader.Read();
                                sb.Append(get);
                            }
                            else if(IsBoundary(get) || IsOperater(get))
                            {
                                state = 2;
                            }
                            else
                            {
                                //错误处理
                                //非法字符处理
                                do
                                {
                                    _StringReader.Read();
                                    sb.Append(get);
                                    if (_StringReader.Peek() != -1)
                                    {
                                        get = (char)_StringReader.Peek();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                while (!IsBoundary(get));
                                throw new NumFormatErrorException(sb.ToString(), line);
                            }
                        }
                        break;
                    case 3:
                        {
                            char get = (char)_StringReader.Peek();
                            if (get >= '0' && get <= '7')
                            {
                                _StringReader.Read();
                                // 八进制
                                sb.Append(get);

                            }
                            else if (get == '.' && sb.Length == 1)
                            {
                                state = 8;
                                _StringReader.Read();
                                sb.Append(get);
                            }
                            else if (get == 'x' || get == 'X')
                            {
                                
                                state = 5;
                                sb.Append(get);
                                _StringReader.Read();
                            }
                            else if(IsBoundary(get) || IsOperater(get))
                            {
                                if(sb.Length != 1)
                                {
                                    return new Num(Convert.ToInt32(sb.ToString(), 8), sb.ToString());
                                }
                                else
                                {
                                    state = 4;
                                }
                            }
                            else
                            {
                                //错误处理
                                //非法字符处理
                                do
                                {
                                    _StringReader.Read();
                                    sb.Append(get);
                                    if(_StringReader.Peek() != -1)
                                    {
                                        get = (char)_StringReader.Peek();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                while (!IsBoundary(get));
                                throw new NumFormatErrorException(sb.ToString(), line);
                            }

                        }
                        break;
                    case 5:
                        // 十六进制
                        {
                            char get = (char)_StringReader.Peek();
                            if (get >= '0' && get <= '9' || get >= 'a' && get <='f' || get >= 'A' && get <= 'F')
                            {
                                _StringReader.Read();
                                sb.Append(get);

                            }
                            else if(IsBoundary(get) || IsOperater(get))
                            {
                                return new Num(Convert.ToInt32(sb.ToString(), 16), sb.ToString());
                            }
                            else
                            {
                                // 错误处理
                                //非法字符处理
                                do
                                {
                                    _StringReader.Read();
                                    sb.Append(get);
                                    get = (char)_StringReader.Peek();
                                }
                                while (!IsBoundary(get));
                                throw new NumFormatErrorException(sb.ToString(), line);
                            }
                        }
                        break;
                    case 6:
                        break;
                    case 8:
                        {
                            char get = (char)_StringReader.Peek();
                            int a = _StringReader.Peek();

                            if (char.IsDigit(get))
                            {
                               
                                _StringReader.Read();
                                sb.Append(get);
                            }
                            else if(get == 'e')
                            {
                                _StringReader.Read();
                                sb.Append(get);
                                state = 9;
                            }
                            else if (IsBoundary(get) || IsOperater(get) || a == -1)
                            {
                                Real r = new Real(double.Parse(sb.ToString()));
                                r.Line = line;
                                return r;
                            }
                            else
                            {
                                // 错误处理
                                //非法字符处理
                                do
                                {
                                    _StringReader.Read();
                                    sb.Append(get);
                                    get = (char)_StringReader.Peek();
                                }
                                while (!IsBoundary(get));
                                throw new NumFormatErrorException(sb.ToString(), line);

                            }

                            

                        }
                        break;
                    case 9:
                        {
                            char get = (char)_StringReader.Peek();
                            int a = _StringReader.Peek();
                            if (sb[sb.Length - 1] == 'e' && (get == '+' || get == '-'))
                            {
                                _StringReader.Read();
                                sb.Append(get);
                            }


                            if (char.IsDigit(get))
                            {
                                _StringReader.Read();
                                sb.Append(get);
                            }
                            else if (IsBoundary(get) || IsOperater(get) || a == -1 || get == '\r')
                            {
                                
                                try
                                {
                                    Real r = new Real(double.Parse(sb.ToString()), sb.ToString());
                                    r.Line = line;
                                    return r;
                                }
                                catch(FormatException e)
                                {
                                    ErrorString.Append($"在第{line}行出现格式错误: {sb.ToString()}\n");
                                    return null;
                                }
                                
                            }
                            else
                            {
                                return null;
                            }

                        }
                        break;
                }
            }
            return new Num(int.Parse(sb.ToString()));

        }

        Token? ScanComment(char ch, ref Token? start, ref Token? end)
        {

            int state = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(ch);
            while(state != 3 && state != 7)
            {
                switch(state)
                {
                    case 0: state = 1; break;
                    case 1:
                        {
                            char get = (char)_StringReader.Peek();
                            if (get == '/')
                            {
                                state = 2;

                            }
                            else if(get == '*')
                            {
                                state = 4;
                            }
                            else
                            {
                                state = 3;
                            }
                        }
                        break;
                    case 2:
                        {
                            int get;
                            do
                            {
                                _StringReader.Read();
                                get = _StringReader.Peek();

                            } while((char)get != '\n' && get != -1);
                            Word w = new Word("//", Tag.CMT);
                            w.Line = line;
                            return w;
                        }
                    case 4:
                        {

                            start = new Word("/*", Tag.BCMT);
                            start.Line = line;
                            _StringReader.Read();
                            int get = _StringReader.Peek();
                            while(get != -1)
                            {
                                if((char)get == '*')
                                {
                                    char next = (char)_StringReader.Peek();
                                    get = _StringReader.Read();
                                    if(next == '/')
                                    {

                                        end = new Word("*/", Tag.BCMTE);
                                        end.Line = line;
                                        break;
                                    }
                                    
                                }
                                else
                                {
                                    get = _StringReader.Read();
                                    if ((char)get == '\n')
                                        line++;
                                }

                            }
                            // 已经读到末尾了
                            return new Word("我吐了", Tag.ERROR);
                        }
                }
            }
            return new Token('/');
           

        }

        Word ScanChar(char ch)
        {
            int state = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(ch);
            int a = _StringReader.Peek();
            while (state != 3 && a != -1)
            {
                switch(state)
                {
                    case 0: state = 1; break;
                    case 1:
                        {
                            char get = (char)_StringReader.Read();
                            sb.Append(get);
                            state = 2;
                        }
                        break;
                    case 2:
                        {
                            char get = (char)_StringReader.Peek();
                            if(get == '\'')
                            {
                                state = 3;
                                sb.Append(get);
                                _StringReader.Read();
                            }
                            else
                            {
                                throw new MissingEndCharException(ch, '\'', line);
                            }
                        }
                        break;

                }
            }

            if(a == -1)
            {
                throw new MissingEndCharException(ch, '\'', line);
            }





            return new Word(sb.ToString(), Tag.CHAR);
        }

        Word ScanString(char ch)
        {
            int state = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(ch);
            while (state != 2)
            {
                switch (state)
                {
                    case 0: state = 1; break;
                    case 1:
                        {
                            char get = (char)_StringReader.Peek();
                            do
                            {
                                _StringReader.Read();
                                sb.Append(get);
                                get = (char)_StringReader.Peek();
                                if(IsBoundary(get) || IsOperater(get))
                                {
                                    return new Word(sb.ToString(), Tag.ERROR);
                                }
                            } while (get != '"') ;
                            sb.Append(get);
                            return new Word(sb.ToString(), Tag.STRING);
                        }
                }
            }
            return new Word(sb.ToString(), Tag.STRING);

        }

        char GetPair(char ch)
        {
            switch(ch) 
            {
                case '{': return '}';
                case '(': return ')';
                case '[': return ']';
                case '}': return '{';
                case ']': return '[';
                case ')': return '(';
            }

            return ch;
        }

        void CheckPair(List<Token> s)
        {
            Stack<Token> stack = new Stack<Token>();



            stack.Push(s[0]);
            int i = 1;
            Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
            while (stack.Count != 0 && i < s.Count)
            {
                Token right = s[i++]; // 闭合符号
                Token top = stack.Peek();

                



                if((char)right.tag == '}' || (char)right.tag == ')' || (char)right.tag == ']')
                {
                    if ((char)right.tag == '}' && (char)top.tag == '{')
                    {
                        stack.Pop();
                        if(i < s.Count)
                            stack.Push(s[i++]);
                        continue;
                    }
                    if ((char)right.tag == ')' && (char)top.tag == '(')
                    {
                        stack.Pop();
                        if (i < s.Count)
                            stack.Push(s[i++]);
                        continue;
                    }
                    if((char)right.tag == ']' && (char)top.tag == '[')
                    {
                        stack.Pop();
                        if (i < s.Count)
                            stack.Push(s[i++]);
                        continue;
                    }

                    if((char)right.tag == '}')
                    {
                        bool flag = false;
                        foreach (var item in stack)
                        {
                            if ((char)item.tag == '{')
                            {
                                flag = true;
                                break;
                            }
                        }
                        if(flag)
                        {
                            Token t;
                            while(stack.Count != 0)
                            {
                                t = stack.Pop();
                                if((char)t.tag == '{')
                                {
                                    break;
                                }
                                else
                                {
                                    // 对中途未匹配到的抛出异常
                                    //ErrorString.Append($"在第{t.Line}行'{(char)t.tag}'没有闭合字符: '{GetPair((char)t.tag)}'\n");
                                    keyValuePairs.Add($"在第{t.Line}行'{(char)t.tag}'没有闭合字符: '{GetPair((char)t.tag)}'\n", t.Line);
                                }
                            }

                        }
                    }


                    if ((char)right.tag == ')')
                    {
                        bool flag = false;
                        foreach (var item in stack)
                        {
                            if ((char)item.tag == '(')
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            Token t;
                            while (stack.Count != 0)
                            {
                                t = stack.Pop();
                                if ((char)t.tag == '(')
                                {
                                    break;
                                }
                                else
                                {
                                    // 对中途未匹配到的抛出异常
                                    //ErrorString.Append($"在第{t.Line}行'{(char)t.tag}'没有闭合字符: '{GetPair((char)t.tag)}'\n");
                                    keyValuePairs.Add($"在第{t.Line}行'{(char)t.tag}'没有闭合字符: '{GetPair((char)t.tag)}'\n", t.Line);
                                }
                            }

                        }
                    }

                    if ((char)right.tag == ']')
                    {
                        bool flag = false;
                        foreach (var item in stack)
                        {
                            if ((char)item.tag == '[')
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            Token t;
                            while (stack.Count != 0)
                            {
                                t = stack.Pop();
                                if ((char)t.tag == '[')
                                {
                                    break;
                                }
                                else
                                {
                                    // 对中途未匹配到的抛出异常
                                    //ErrorString.Append($"在第{t.Line}行'{(char)t.tag}'没有闭合字符: '{GetPair((char)t.tag)}'\n");
                                    keyValuePairs.Add($"在第{t.Line}行'{(char)t.tag}'没有闭合字符: '{GetPair((char)t.tag)}'\n", t.Line);
                                }
                            }

                        }
                    }
                }
                else
                {
                    stack.Push(right);
                }
            }

            while (stack.Count != 0)
            {
                Token t = stack.Pop();
                //ErrorString.Append($"在第{t.Line}行'{(char)t.tag}'没有闭合字符: '{GetPair((char)t.tag)}'\n");
                keyValuePairs.Add($"在第{t.Line}行'{(char)t.tag}'没有闭合字符: '{GetPair((char)t.tag)}'\n", t.Line);
            }

            var sortedByValue = keyValuePairs.OrderBy(kvp => kvp.Value);
            foreach (var item in sortedByValue)
            {
                ErrorString.Append(item.Key);
            }
        }

        Token? ScanBondaryAndOperator(char ch)
        {
            char next = (char)_StringReader.Peek();
            if (IsOperater(next))
            {
                /*char nnext = (char)_StringReader.Peek();
                if(IsOperater(nnext))
                {
                    throw new IlleagalOperatorException($"{ch}{next}{nnext}", line);
                }*/
                switch (ch)
                {
                    case '&':
                        if (next.Equals('&')) return Word.and;break;
                    case '|':
                        if (next.Equals('|')) return Word.or; break;
                    case '=':
                        if (next.Equals('=')) return Word.eq; break;
                    case '!':
                        if (next.Equals('=')) return Word.ne; break;
                    case '<':
                        if (next.Equals('=')) return Word.lt; break;
                    case '>':
                        if (next.Equals('=')) return Word.gt; break;
                    case '+':
                        {
                            if(next.Equals('+'))
                                return Word.inc;
                            if(next.Equals('='))
                            {
                                return Word.incEq;
                            }
                        }
                        break;
                    

                }
                if(ch != '(' && next != ')')
                    throw new IlleagalOperatorException($"{ch}{next}", line);
            }
            

            if(ch == '-' && char.IsDigit(next))
            {
                _StringReader.Read();
                Num a = (Num)ScanNum(next);
                return new Num(0 - a.value);
            }

            if(ch == '(' || ch == ')' || ch == '[' || ch == ']' || ch == '{' || ch == '}')
            {
                Token t = new Token(ch);
                pair.Add(t);
                return t;
            }


            return new Token(ch);

        }

        bool IsBlank(char ch)
        {

            if (ch == ' ' || ch == '\n' || ch == '\t' || ch == '\r')
                return true;
            return false;

        }

        void ScanBlank(char ch)
        {
            int state = 0;
            while(state != 2)
            {
                switch(state) 
                {
                    case 0:
                        { 
                            if(ch == '\n')
                            {
                                line++;
                            }
                            state = 1;
                        }
                        break;
                    case 1:
                        {
                            char get = (char)_StringReader.Peek();
                            while(IsBlank(get))
                            {
                                _StringReader.Read();
                                if (get == '\n')
                                {
                                    line++;
                                }
                                get = (char)_StringReader.Peek();
                            }
                            state = 2;
                        }
                        break;
                }
            }


        }

        void ScanIllegalChar(char ch)
        {
            
            StringBuilder sb = new StringBuilder();
            sb.Append(ch);
            /*int a = _StringReader.Peek();
            while (a != -1)
            {
                if(IsBoundary((char)a) || IsBlank((char)a))
                {
                    break;
                }
                sb.Append((char)a);
                _StringReader.Read();
                a = _StringReader.Peek();
            } */

            /*if(sb.Length != 1)
            // 非法字符
                throw new IlleagalSymbolException(sb.ToString(), ch, line);
            else*/
                throw new IlleagalSymbolException(null, ch, line);
        }
        public List<Token?>? NewScan()
        {
            List<Token?>? tokens = new List<Token?>();
            int a = _StringReader.Peek();

            while(a != -1)
            {
                char ch = (char)_StringReader.Read();
                if(IsBlank(ch))
                {
                    ScanBlank(ch);
                }
                else if (char.IsLetter(ch) || ch == '_')
                {
                    Word w = ScanWord(ch);
                    tokens.Add(w);
                }
                else if (char.IsDigit(ch))
                {

                    try
                    {
                        Token? n = ScanNum(ch);
                        if (n != null)
                        {
                            n.Line = line;
                            tokens.Add(n);
                        }
                    }
                    catch (NumFormatErrorException e)
                    {

                        ErrorString.Append(e.Message);
                    }
                    
                    
                    
                }
                else if (ch == '/')
                {
                    Token? start = null;
                    Token? end = null;
                    Token? t = ScanComment(ch, ref start, ref end);
                    if (start != null)
                    {
                        //tokens.Add(start);
                        //tokens.Add(end);
                    }
                    else
                    {
                        //tokens.Add(t);
                    }
                }
                else if (ch == '\'')
                {

                    try
                    {
                        Word w = ScanChar(ch);
                        tokens.Add(w);
                    }
                    catch (MissingEndCharException e)
                    {

                        ErrorString.Append(e.Message);
                    }
                    
                }
                else if (ch == '"')
                {
                    Word w = ScanString(ch);
                    tokens.Add(w);
                }
                else if(IsBoundary(ch) || IsOperater(ch))
                {
                    try
                    {
                        //界符和运算符
                        Token? t = ScanBondaryAndOperator(ch);
                        t.Line = line;
                        tokens.Add(t);

                    }
                    catch (IlleagalOperatorException e)
                    {

                        ErrorString.Append(e.Message);
                    }
                    
                }
                else
                {   // 非法字符检测
                    try
                    {
                        ScanIllegalChar(ch);

                    } 
                    catch(IlleagalSymbolException e) 
                    {
                        ErrorString.Append(e.Message);
                    }
                    
                    
                }
                a = _StringReader.Peek();
            }
            if(pair.Count != 0)
                CheckPair(pair);

            return tokens;
        }
#endif




    }
}
