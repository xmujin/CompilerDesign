
using myapp.Model.Inter;
using myapp.Model.Lexer;
using myapp.Model.Symbols;
using myapp.Model.Utils;
using Type = myapp.Model.Symbols.Type;


namespace myapp.Model.Parser
{

#if true

#pragma warning disable
    public class Parser
    {

        Lexer.Lexer lex;
        Token look;

        // 当前的符号表
        Env top = null;
        int used = 0; // 用于变量声明的存储位置
        int count = 0;
        Dictionary<Token, Function> funcs = new Dictionary<Token, Function>();

        TreeNode root;
        public TreeNode GetTree()
        {
            return root;
        }

        void Error(string s)
        {
            
        }
        void Match(int t)
        {
            if (look.tag == t)
            {
                Move();
            }    
            else Error("语法错误");
        }

        public Parser(Lexer.Lexer l)
        {
            lex = l;
            Move(); // 读取第一个词法单元
           
        }

      

        void Move()
        {
            // 这里读取一个词法单元赋值给向前看词法单元
            look = lex.Scan();
        }


        /// <summary>
        /// 运行语法分析  program -> block
        /// </summary>
        public void Program()
        {
            root = new TreeNode("program");

            // 创建顶层符号表
            top = new Env();
            GdeclFdecls(root.AddChild("GdeclFdecls"));
            /*int begin = s.Newlabel();
            int after = s.Newlabel();
            s.Emitlabel(begin);
            s.Gen(begin, after);
            s.Emitlabel(after);*/
            //TreeNode root = CreateBlock(s);
        }

        




        #region program

        /// <summary>
        /// 识别类型
        /// </summary>
        /// <returns></returns>
        Type _Type_()
        {
            Type p = (Type)look;
            Match(Tag.BASIC);
            if (look.tag != '[') return p;
            else return null;
        }


        void GdeclFdecls(TreeNode root)
        {
            if(look == null)
            {
                root.AddChild("空语句");
            }
            else if(look.tag == Tag.BASIC)
            {
                
                GdeclFdecl(root.AddChild("GdeclFdecl"));
                GdeclFdecls(root.AddChild("GdeclFdecls"));
            }
            else
            {
                //todo 起始错误处理
            }
            
        }

        void GdeclFdecl(TreeNode root)
        {
            Type curType = _Type_();
            Token tok = look; // 获取标识符
            Match(Tag.ID);
            root.AddChild(((Word)curType).lexeme, ((Word)tok).lexeme);
            Id id = new Id((Word)tok, curType, used); // 将标识符及标识符对应的类型信息存入到符号表中
            top.Put(tok, id);
            used = used + curType.width;

            if (look.tag == '(')
            {
                Fdecl(root.AddChild("fdecl"));
            }
            else
            {
                Gdecl(root.AddChild("gdecl"), curType);
            }

            

        }

        void Gdecl(TreeNode root, Type topType)
        {
            if (look.tag == ';')
            {
                Match(';');
                root.AddChild(";");
            }
            else if (look.tag == ',')
            {
                Match(',');
                Token t = look; // 获取标识符
                Match(Tag.ID);
                Id id = new Id((Word)t, topType, used); // 将标识符及标识符对应的类型信息存入到符号表中
                top.Put(t, id);
                used = used + topType.width;
                root.AddChild(",", ((Word)t).lexeme);

                Gdecl(root.AddChild("gdecl"), topType);
            }
            else if(look.tag == '=')
            {
                Match('=');
                root.AddChild("=");
                Bool(root.AddChild("bool"));
                Gdecl(root.AddChild("gdecl"), topType);

            }
        }


        void Fdecl(TreeNode root)
        {
            if(look.tag == '(')
            {
                Move();
                root.AddChild("(");
                Paramlist(root.AddChild("paramlist"));
                Match(')');
                root.AddChild(")");
                Block(root.AddChild("block"));
            }
        }


        void Paramlist(TreeNode root)
        {

            if (look.tag == Tag.BASIC)
            {
                Type curType = _Type_();
                Token tok = look; // 获取标识符
                Match(Tag.ID);
                Paramlist_(root.AddChild("Paramlist'"));

            }
            else
            {

            }
        }

        void Paramlist_(TreeNode root)
        {

            if (look.tag == ',')
            {
                Move();
                root.AddChild(",");
                Paramlist(root.AddChild("paramlist"));

            }
            else
            {

            }
        }


        void Block(TreeNode root)
        {
            Match('{');
            root.AddChild("{");
            Statements(root.AddChild("statements"));
            Match('}');
            root.AddChild("}");
        }

        void Statements(TreeNode root)
        {
            if(look.tag == '}')
            {
                return;
            }
            Statement(root.AddChild("statement"));
            Statements(root.AddChild("statements"));
        }

        void Statement(TreeNode root)
        {
            if(look.tag == Tag.BASIC)
            {
                GdeclFdecl(root.AddChild("gdeclFdecl"));
            }
            else if(look.tag == Tag.ID)
            {
                Word w = (Word)look;
                root.AddChild(w.lexeme);
                Match(Tag.ID);
                Match('=');
                root.AddChild("=");
                Bool(root.AddChild("bool"));
                Match(';');
                root.AddChild(";");
            }
            else if(look.tag == Tag.FOR)
            {
                Match(Tag.FOR);
                root.AddChild("for");
                Match('(');
                root.AddChild("(");
                Statement(root.AddChild("statement"));
                Match(';');
                root.AddChild(";");
                Bool(root.AddChild("bool"));
                Match(';');
                root.AddChild(";");
                Statement(root.AddChild("statement"));
                Match(')');
                root.AddChild(")");
                Statement(root.AddChild("statement"));
            }
            else if(look.tag == Tag.IF)
            {
                Move();
                root.AddChild("if");
                Match('(');
                root.AddChild("(");
                Bool(root.AddChild("bool"));
                Match(')');
                root.AddChild(")");
                Statement(root.AddChild("statement"));
                if(look.tag == Tag.ELSE)
                {
                    root.AddChild("else");
                    Statement(root.AddChild("statement"));
                }
            }
            else if (look.tag == Tag.WHILE)
            {
                Match(Tag.WHILE);
                root.AddChild("while");
                Match('(');
                root.AddChild("(");
                Bool(root.AddChild("bool"));
                Match(')');
                root.AddChild(")");
                Statement(root.AddChild("statement"));
            }
            else if (look.tag == Tag.DO)
            {
                Match(Tag.DO);
                root.AddChild("do");
                Statement(root.AddChild("statement"));
                Match(Tag.WHILE);
                root.AddChild("while");
                Match('(');
                root.AddChild("(");
                Bool(root.AddChild("bool"));
                Match(')');
                root.AddChild(")");
                Statement(root.AddChild("statement"));
                Match(';');
                root.AddChild(";");
            }
            else if (look.tag == Tag.BREAK)
            {
                Match(Tag.BREAK);
                Match(';');
                root.AddChild("break", ";");
            }
            else if(look.tag == '{')
            {

                Block(root.AddChild("block"));
            }
            else
            {

            }

        }

        #endregion
        #region newbool

        /// <summary>
        /// ||语句
        /// Bool  -> Join Bool'
        /// Bool' -> || Join Bool'
        ///          | ε
        /// </summary>
        /// <returns></returns>
        void Bool(TreeNode root)
        {
            Join(root.AddChild("Join"));
            Bool_(root.AddChild("Bool'"));
        }

        void Bool_(TreeNode root)
        {
            if (look.tag == Tag.OR)
            {
                Match(Tag.OR);
                root.AddChild("||");
                Join(root.AddChild("Join"));
                Bool_(root.AddChild("Bool'"));
            }
            else
            {
                root.AddChild("empty");
            }
        }

        /// <summary>
        /// &&语句
        /// Join  -> Equality Join'
        /// Join' -> && Equality Join'
        ///          | ε
        /// </summary>
        /// <returns></returns>
        void Join(TreeNode root)
        {
            Equality(root.AddChild("Equality"));
            Join_(root.AddChild("Join'"));
        }

        void Join_(TreeNode root)
        {
            if (look.tag == Tag.AND)
            {
                Match(Tag.AND);
                root.AddChild("&&");
                Equality(root.AddChild("Equality"));
                Join_(root.AddChild("Join'"));
            }
            else
            {
                root.AddChild("empty");
            }
        }

        /// <summary>
        /// 语句
        /// Equality  -> Compare Equality'
        /// Equality' -> == Compare Equality'
        ///          | != Compare Equality'
        ///          | ε
        /// </summary>
        /// <returns></returns>
        void Equality(TreeNode root)
        {
            Compare(root.AddChild("Compare"));
            Equality_(root.AddChild("Equality'"));
        }

        void Equality_(TreeNode root)
        {
            if (look.tag == Tag.EQ || look.tag == Tag.NE)
            {
                Word w = (Word)look;
                Move();
                root.AddChild(w.lexeme);
                Compare(root.AddChild("Compare"));
                Equality_(root.AddChild("Equality'"));
            }
            else
            {
                root.AddChild("empty");
            }
        }

        /// <summary>
        /// 语句
        /// Compare  -> Expr Compare'
        /// Compare' -> < Expr Compare'
        ///          | <= Expr Compare'
        ///          | > Expr Compare'
        ///          | >= Expr Compare'
        ///          | ε
        /// </summary>
        /// <returns></returns>
        void Compare(TreeNode root)
        {
            Expr(root.AddChild("Expr"));
            Compare_(root.AddChild("Compare'"));
        }

        void Compare_(TreeNode root)
        {
            if (look.tag == '<' || look.tag == Tag.LT || look.tag == '>' || look.tag == Tag.GT)
            {

                if(look.tag == Tag.GT || look.tag == Tag.LT)
                {
                    Word w = (Word)look;
                    root.AddChild(w.lexeme);
                }
                else
                {
                    root.AddChild("" + (char)look.tag);
                }
                Move();
                
                Expr(root.AddChild("Expr"));
                Compare_(root.AddChild("Compare'"));
            }
            else
            {
                root.AddChild("empty");
            }
        }

        /// <summary>
        /// 语句
        /// Expr  -> Term Expr'
        /// Expr' -> + Term Expr'
        ///          | - Term Expr'
        ///          | ε
        /// </summary>
        /// <returns></returns>
        void Expr(TreeNode root)
        {
            Term(root.AddChild("Term"));
            Expr_(root.AddChild("Expr'"));
        }

        void Expr_(TreeNode root)
        {
            if (look.tag == '+' || look.tag == '-')
            {
                root.AddChild("" + (char)look.tag);
                Move();
                Term(root.AddChild("Term"));
                Expr_(root.AddChild("Expr'"));
            }
            else
            {
                root.AddChild("empty");
            }
        }

        /// <summary>
        /// 语句
        /// Term  -> Unary Term'
        /// Term' -> * Unary Term'
        ///          | / Unary Term'
        ///          | ε
        /// </summary>
        /// <returns></returns>
        void Term(TreeNode root)
        {
            Unary(root.AddChild("Unary"));
            Term_(root.AddChild("Term'"));
        }

        void Term_(TreeNode root)
        {
            if (look.tag == '*' || look.tag == '/')
            {
                root.AddChild("" + (char)look.tag);
                Move();
                Unary(root.AddChild("Unary"));
                Term_(root.AddChild("Term'"));
            }
            else
            {
                root.AddChild("empty");
            }
        }

        /// <summary>
        /// 语句
        /// Unary -> !  Unary
        ///          | -  Unary
        ///          | Factor
        /// </summary>
        /// <returns></returns>
        void Unary(TreeNode root)
        {
            if (look.tag == '!' || look.tag == '-')
            {
                root.AddChild("" + (char)look.tag);
                Move();
                Unary(root.AddChild("Unary"));
            }
            else
            {
                Factor(root.AddChild("Factor"));
            }
        }


        /// <summary>
        /// 该函数用于处理因子
        /// </summary>
        /// <returns></returns>
        void Factor(TreeNode root)
        {
            //Expr x = null;
            switch (look.tag)
            {
                case '(':
                    {
                        Move();
                        root.AddChild("(");
                        Bool(root.AddChild("bool"));
                        Match(')');
                        root.AddChild(")");

                    }
                    break;
                case Tag.NUM:
                    {
                        //x = new Constant(look, Type.Int);
                        Num w = (Num)look;
                        root.AddChild("" + w.value);
                        Move();
                        //return x;
                    }
                    break;

                case Tag.REAL:
                    {
                        Num w = (Num)look;
                        root.AddChild("" + w.value);
                        Move();
                        //return x;
                    }
                    break;
                case Tag.TRUE:
                    {
                        Word w = (Word)look;
                        root.AddChild("" + w.lexeme);
                        Move();
                        //return x;
                    }
                    break;
                case Tag.FALSE:
                    {
                        Word w = (Word)look;
                        root.AddChild("" + w.lexeme);

                        Move();
                        //return x;
                    }
                    break;
                case Tag.ID:
                    {
                        /*
                         * 这里的look是已有的，原因是词法分析时对于同样的标识符，
                         * 所对应的Token的引用是相同的
                         * **/
                        Id id = top.Get(look);
                        Word w = (Word)look;
                        root.AddChild("" + w.lexeme);
                        if (id == null)
                        {
                            // Todo 未声明的错误
                        }
                        Move();


                    }

                    break;
                default:
                    Error("语法错误");
                    break;
                    //return x;


            }
        }



        #endregion








        // block -> { decls stmts }
        /*Stmt Block()
        {
            Match('{');
            // 保存当前符号表的引用
            Env savedEnv = top;
            top = new Env(top);
            Decls();
            Stmt s = Stmts();
            
            Match('}');
          

            top = savedEnv;
            return s;
        }
        */



#region olddecl
        /// <summary>
        /// 声明语句   D -> type ID;
        /// </summary>
        void Decls()
        {
            while (look.tag == Tag.BASIC)
            {
                int index = lex.Index - 1;
                Type p = _Type_();
                Token tok = look; // 获取标识符
                Match(Tag.ID);
                Id id = new Id((Word)tok, p, used);
                // 将标识符及标识符对应的类型信息存入到符号表中
                top.Put(tok, id);
                used = used + p.width;

                if(look.tag == ',')
                {
                    while (look.tag == ',')
                    {
                        Match(',');
                        Token t = look; // 获取标识符
                        Id eid = new Id((Word)t, p, used);
                        Match(Tag.ID);
                        top.Put(t, eid);
                    }
                    Match(';');

                }
                else if(look.tag == '(') // 遇到函数声明，回退
                {

                    
                    lex.Index = index;
                    Move();

                    break;
                }
                else
                {
                    Match(';');
                }

            }
        }

        void Decl()
        {
            int cur = lex.Index - 1;
            Type p = _Type_();
            Token tok = look; // 获取标识符
            Match(Tag.ID);

            if(look.tag == ',')
            {
                while(look.tag == ',')
                {
                    Match(',');
                    Token t = look; // 获取标识符
                    Id id = new Id((Word)t, p, used);
                    Match(Tag.ID);
                    top.Put(t, id);
                }
                Match(';');
            }
            else if(look.tag == ';')
            {
                Id id = new Id((Word)tok, p, used);
                top.Put(tok, id);

            }
            if(look.tag == '(')
            {
                lex.Index = cur;
                Move();
            }


        }
        #endregion


        /*
        void Functions()
        {
            while (look != null && look.tag == Tag.BASIC)
            {
                Type p = _Type_();
                Token tok = look; // 获取标识符
                Match(Tag.ID);
                Match('(');
                List<Id> param = Param();
                Match(')');
                funcs.Add(tok, new Function(p, param, _Stmt_()));
            }
        }

        List<Id> Param()
        {
            List<Id> list = new List<Id>();
            while(look.tag == Tag.BASIC)
            {
                Type p = _Type_();
                Token tok = look; // 获取标识符
                Match(Tag.ID);
                list.Add(new Id((Word)tok, p, used));
                if(look.tag != ',')
                {
                    break;
                }
                Match(',');
            }

            return list;
        }


        


        /// <summary>
        /// 数组的维度
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        Type Dims(Type p)
        {
            Match('[');
            Token tok = look;
            Match(Tag.NUM);
            Match(']');
            if(look.tag == '[')
            {
                p = Dims(p);
            }
            return new Array(((Num)tok).value, p);
        }


        /*
         // stmts -> stmt stmts
         // stmts -> e
        Stmt Stmts()
        {
            // 直到读取到空语句
            if (look.tag == '}') return Stmt.Null;
            else
            {

                return new Seq(_Stmt_(), Stmts());
            }
                
        }


        /// <summary>
        /// stmt -> .........
        /// </summary>
        /// <returns></returns>
        Stmt _Stmt_()
        {
            Expr x;
            Stmt s, s1, s2;
            Stmt savedStmt;   // 用于为break语句保存外层的循环语句
            switch(look.tag)
            {
                case ';': Move(); return Stmt.Null;
                case Tag.IF:
                    {
                        Match(Tag.IF);
                        Match('(');
                        x = Bool();
                        Match(')');
                        s1 = _Stmt_();
                        if(look.tag != Tag.ELSE) 
                        {
                            return new If(x, s1);
                        }
                        Match(Tag.ELSE);
                        s2 = _Stmt_();
                        return new Else(x, s1, s2);
                    }

                case Tag.WHILE:
                    {
                        While whilenode = new While();
                        savedStmt = Stmt.Enclosing;
                        Stmt.Enclosing = whilenode;
                        Match(Tag.WHILE);
                        Match('(');
                        x = Bool();
                        Match(')');
                        s1 = _Stmt_();
                        whilenode.Init(x, s1);
                        Stmt.Enclosing = savedStmt;
                        return whilenode;
                    }
                case Tag.DO:
                    {
                        Do donode = new Do();
                        savedStmt = Stmt.Enclosing;
                        Stmt.Enclosing = donode;
                        Match(Tag.DO);
                        s1 = _Stmt_();
                        Match(Tag.WHILE);
                        Match('(');
                        x = Bool();
                        Match(')');
                        Match(';');
                        donode.Init(s1, x);
                        Stmt.Enclosing = savedStmt;
                        return donode;
                    }
                case Tag.FOR:
                    {
                        Match(Tag.FOR);
                        Match('(');
                        Stmt assign = _Stmt_();


                        Expr exp = Bool();
                        Match(';');

                        Token t = look;
                        Match(Tag.ID);
                        Id id = top.Get(t);
                        Match('=');
                        Stmt end = new Set(id, Bool());
                        Match(')');
                        Stmt block = _Stmt_();
                        For fornode = new For(assign, exp, end, block);
                        return fornode;
                    }


                case Tag.BREAK:
                    {
                        Match(Tag.BREAK);
                        Match(';');
                        return new Break();
                    }

                case '{':
                    {

                        return Block();
                    }
                    
                default:
                    {
                        return Assign(); // stmt -> Assign

                    }
                    
                   
                        
            }



        }


        // 赋值语句代码相关
        /// <summary>
        /// assign -> id = Expr
        /// </summary>
        /// <returns></returns>
        Stmt Assign()  
        {
            Stmt stmt;
            Token t = look;
            Match(Tag.ID);
            Id id = top.Get(t);


            if(id == null)
            {
                // 发出未声明的错误
            }
            if(look.tag == '=') // id = Expr
            {
               
                Move();
                stmt = new Set(id, Bool());

             
                // 这里测试因子
                //stmt = new Set(id, _Expr_());

            }
            else
            {
                Access x = Offset(id);
                Match('=');
                stmt = new SetElem(x, Bool());
            }

            // 语句以分号结尾
            Match(';');
            return stmt;
        
        }

        */











#if false



        /*void Join(TreeNode root)
        {
            Expr x = Equality();
            while(look.tag == Tag.AND)
            {
                Token tok = look;  
                Move();
                x = new And(tok, x, Equality());

            }
            return x;
        }*/

        /// <summary>
        /// 处理== !=
        /// </summary>
        /// <returns></returns>
        Expr Equality()
        {
            Expr x = Rel();
            while (look.tag == Tag.EQ || look.tag == Tag.NE)
            {
                Token tok = look;
                Move();
                x = new Rel(tok, x, Rel());

            }
            return x;
        }


        /// <summary>
        /// 处理关系运算符（如大于、大于等于，小于等于）
        /// </summary>
        /// <returns></returns>
        Expr Rel()
        {
            Expr x = _Expr_();
            switch(look.tag)
            {
                case '<':
                case Tag.LT:
                case '>':
                case Tag.GT:
                    {
                        Token tok = look;
                        Move();
                        return new Rel(tok, x, _Expr_());
                    }
                default:
                    return x;
            }
        }


        /// <summary>
        /// 处理加法和减法,因子被视为常亮, expr -> expr + term | expr - term | term
        /// </summary>
        /// <returns></returns>
        Expr _Expr_()
        {
            Expr x = Term();
            while(look.tag == '+' || look.tag == '-')
            {
                Token tok = look;
                Move();
                x = new Arith(tok, x, Term());
            }
            return x;
        }


        /// <summary>
        /// 处理乘法和除法  term -> term * unary | term / unary | unary
        /// </summary>
        /// <returns></returns>
        Expr Term()
        {
            Expr x = _Unary_();
            while (look.tag == '*' || look.tag == '/')
            {
                Token tok = look;
                Move();
                x = new Arith(tok, x, _Unary_());
            }
            return x;
        }

        /// <summary>
        /// 处理一元运算符，如负号，逻辑非等,若不需要处理，则返回因子
        /// </summary>
        /// <returns></returns>
        Expr _Unary_()
        {
            if(look.tag == '-') // 处理负数
            {
                Move();
                return new Unary(Word.minus, _Unary_());
            }
            else if(look.tag == '!')
            {
                Token tok = look;
                Move();
                return new Not(tok, _Unary_());
            }
            else return Factor();
        }

        /// <summary>
        /// 该函数用于处理因子
        /// </summary>
        /// <returns></returns>
        Expr Factor()
        {
            Expr x = null;
            switch (look.tag)
            {
                case '(':
                    {
                        Move();
                        x = Bool();
                        Match(')');
                        return x;
                    }
                case Tag.NUM:
                    {
                        x = new Constant(look, Type.Int);
                        Move();
                        return x;
                    }
                    
                case Tag.REAL:
                    {
                        x = new Constant(look, Type.Float);
                        Move();
                        return x;
                    }
                case Tag.TRUE:
                    {
                        x = Constant.True;
                        Move();
                        return x;
                    }
                case Tag.FALSE:
                    {
                        x = Constant.False;
                        Move();
                        return x;
                    }
                case Tag.ID:
                    {
                        string s = look.ToString();
                        /*
                         * 这里的look是已有的，原因是词法分析时对于同样的标识符，
                         * 所对应的Token的引用是相同的
                         * **/
                        Id id = top.Get(look); 

                        if(id == null)
                        {
                            // 未声明的错误
                        }
                        Move();

                        // 这里直接返回ID
                        if (look.tag != '[')
                            return id;
                        else return Offset(id);
                    }
                default:
                    Error("语法错误");
                    return x;
                    
                
            }
        }



        Access Offset(Id a)
        {
            Expr i, w, t1, t2, loc;
            Type type = a.type;
            Match('[');
            i = Bool();
            Match(']');
            type = ((Array)type).of;
            w = new Constant(type.width);
            t1 = new Arith(new Token('*'), i, w);
            loc = t1;
            while(look.tag == '[')
            {
                Match('[');
                i = Bool();
                Match(']');
                type = ((Array)type).of;
                w = new Constant(type.width);
                t1 = new Arith(new Token('*'), i, w);
                t2 = new Arith(new Token('+'), loc, t1);
                loc = t2;

            }

            return new Access(a, loc, type);
        }
#endif



    }
#endif
}
