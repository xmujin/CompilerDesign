
using myapp.Model.CodeGen;
using myapp.Model.Inter;
using myapp.Model.Lexer;
using myapp.Model.Utils;
using Newtonsoft.Json;
using Type = myapp.Model.Symbols.Type;


namespace myapp.Model.Parser
{

#if true

#pragma warning disable
    public class Parser
    {

        Lexer.Lexer lex;
        Token look;

        int used = 0; // 用于变量声明的存储位置
        int count = 0;
        //Dictionary<Token, Function> funcs = new Dictionary<Token, Function>();



        public Program js;


        public List<Quadruple> quadruples = new List<Quadruple>();

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
            js = new Program();
            js.body.AddRange(VdeclFdecls());
            //int begin = js.NewLabel();
            //int end = js.NewLabel();
            
            js.Gen(quadruples, 0, 0);



            CodeGen.CodeGen.ShowQuadruple(quadruples);
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



        List<Node> VdeclFdecls()
        {
            List<Node> nodes = new List<Node>();


            if(look == null)
            {
                
            }
            else if(look.tag == Tag.BASIC)
            {

                nodes.Add(VdeclFdecl());
                nodes.AddRange(VdeclFdecls());

            }
            else
            {
                //todo 起始错误处理
            }

            return nodes;
        }

        Node VdeclFdecl()
        {
            Type curType = _Type_();
            Token tok = look; // 获取标识符
            Match(Tag.ID);
            Identifier id = new Identifier(((Word)curType).lexeme, ((Word)tok).lexeme);
            if (look.tag == '(')
            {
                return Fdecl(id);
            }
            else
            {
                return Vdecl(id);
            }

            

        }

        VariableDeclaration Vdecl(Identifier topid)
        {
            VariableDeclaration v = new VariableDeclaration();
            if (look.tag == ';')
            {
                Match(';');
                v.declarations.Add(new VariableDeclarator(topid, null));
                ;
                return v;
            }
            else if (look.tag == ',')
            {
                v.declarations.Add(new VariableDeclarator(topid, null));
                while (look.tag == ',')
                {
                    Match(',');
                    Token t = look; // 获取标识符
                    Match(Tag.ID);
                    if(look.tag != '=')
                    {
                        v.declarations.Add(new VariableDeclarator(new Identifier(topid.idtype, ((Word)t).lexeme), null));
                    }
      
                    else
                    {
                        Match('=');
                        v.declarations.Add(new VariableDeclarator(new Identifier(topid.idtype, ((Word)t).lexeme), Bool()));
                    }
                        
                }
                Match(';');
                return v;
            }
            else if(look.tag == '=') // 赋值语句
            {       
                Match('=');
                v.declarations.Add(new VariableDeclarator(topid, Bool()));


                Match(';');
                return v;
            }

            return v;
        }


        FunctionDeclaration Fdecl( Identifier topid)
        {
            FunctionDeclaration func = new FunctionDeclaration();
            func.id = topid;
            if(look.tag == '(')
            {
                Move();
                func.param = Paramlist();
                Match(')');
                func.body = Block();

            }
            return func;
        }


        List<Identifier> Paramlist()
        {
            List<Identifier> ids = new List<Identifier>();

            if (look.tag == Tag.BASIC)
            {
                Type curType = _Type_();
                Word tok = (Word)look;
                Match(Tag.ID);
                ids.Add(new Identifier(curType.GetTypeStr(), tok.lexeme));
                if(look.tag == ',')
                {
                    Match(',');
                    ids.AddRange(Paramlist());
                }

            }
            else
            {
                return null;
            }
            return ids;
        }

        List<Expression> Argumentlist()
        {
            List<Expression> ids = new List<Expression>();

            if (look.tag == ')')
            {
                return ids;
            }
            else
            {
                while (look.tag != ',' && look.tag != ')')
                {
                    ids.Add(Bool());
                    if (look.tag == ',')
                    {
                        Match(',');
                    }
                    

                }
            }
            
            return ids;
        }


        BlockStatement Block()
        {

            BlockStatement blk = new BlockStatement();
            Match('{');
            ;
            //blk.body
            blk.body = Statements();
            Match('}');
            ;

            return blk;
        }




        List<Node> Statements()
        {
            List<Node> stmts = new List<Node>(); 
            if (look.tag == '}')
            {
                return new List<Node>();
            }
            stmts.Add(Statement());
            List<Node> nodes = Statements();
            if(nodes != null)
            {
                stmts.AddRange(nodes);
            }
            
            
            return stmts;
        }

        Node Statement()
        {
            if(look.tag == Tag.BASIC)  // 变量的声明
            {
                return VdeclFdecl();
            }
            else if(look.tag == ';')
            {

                Match(';');
                return new EmptyStatement();
            }
            else if (look.tag == Tag.IF)
            {
                
                Move();
                Match('(');
                Expression test = Bool();
                Match(')');

                Statement consequent = (Statement)Statement();
                IfStatement ifs = new IfStatement(test, consequent);

                if (look.tag == Tag.ELSE)
                {
                    Move();
                    ifs.alternate = (Statement)Statement();
                }

                return ifs;


            }
            else if(look.tag == Tag.WHILE)
            {
                Move();
                Match('(');

                Expression test = Bool();
                Match(')');

                Statement body = (Statement)Statement();
                WhileStatement ws = new WhileStatement(test, body);
                return ws;
            }
            else if(look.tag == Tag.DO)
            {
                Move();
                Statement body = (Statement)Statement();
                Match(Tag.WHILE);
                Match('(');
                Expression test = Bool();
                Match(')');

                DoWhileStatement dws = new DoWhileStatement(body,test);
                Match(';');
                return dws;
            }
            else if(look.tag == Tag.FOR)
            {
                Move();
                Match('(');
                Statement node = (Statement)Statement();

                Expression test = Bool();
                Match(';');

                Expression update = Assign();

                Match(')');

                Statement body = (Statement)Statement();


                ForStatement ws = new ForStatement(node, test, update, body);

                return ws;
            }
            else if(look.tag == Tag.RETURN) // 返回语句
            {
                Move();
                ReturnStatement returnStatement = new ReturnStatement(Bool()); 
                Match(';');
                return returnStatement;
            }
            else if(look.tag == Tag.BREAK)
            {
                Move();
                Match(';');
                return new BreakStatement();
            }
            else if(look.tag == '{')
            {

                return Block();
            }
            else // 表达式语句, 包括赋值表达式语句， 普通表达表达式语句，以及调用表达式语句
            {

                /*Word w = (Word)look;
                Match(Tag.ID);
                Token op = look;

                if (look.tag == '(')
                {
                    Match('(');
                    CallExpression callExpression = new CallExpression(new Identifier("int", w.lexeme), Argumentlist());
                    // 生成表达式语句
                    ExpressionStatement es1 = new ExpressionStatement(callExpression);
                    Match(')');
                    Match(';');
                    return es1;
                }


                Match('=');
                ExpressionStatement es =
                    new ExpressionStatement(
                        new AssignmentExpression(
                            op, new Identifier("int", w.lexeme), Bool())
                        );*/


                ExpressionStatement es =new ExpressionStatement(Assign());
                Match(';');
                return es;
            }
            

        }

        #endregion



        #region newbool


        Expression Assign()
        {
            Expression x = Bool();
            while (look.tag == '=')
            {
                Token tok = look;
                Move();
                x = new AssignmentExpression(tok, x, Bool());
            }
            return x;
        }




        /// <summary>
        /// ||语句
        /// Bool  -> Join Bool'
        /// Bool' -> || Join Bool'
        ///          | ε
        /// </summary>
        /// <returns></returns>
        Expression Bool()
        {
            Expression x = Join();
            while (look.tag == Tag.OR)
            {
                Token tok = look;
                Move();
                x = new LogicExpression(tok, x, Join());
            }
            return x;
        }

        /// <summary>
        /// &&语句
        /// Join  -> Equality Join'
        /// Join' -> && Equality Join'
        ///          | ε
        /// </summary>
        /// <returns></returns>
        Expression Join()
        {
           
            Expression x = Equality();
            while (look.tag == Tag.AND)
            {
                Token tok = look;
                Move();
                x = new LogicExpression(tok, x, Equality());
            }
            return x;
        }

        /// <summary>
        /// 语句
        /// Equality  -> Compare Equality'
        /// Equality' -> == Compare Equality'
        ///          | != Compare Equality'
        ///          | ε
        /// </summary>
        /// <returns></returns>
        Expression Equality()
        {
          

            Expression x = Compare();
            while (look.tag == Tag.EQ || look.tag == Tag.NE)
            {
                Token tok = look;
                Move();
                x = new BinaryExpression(tok, x, Compare());
            }
            return x;


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
        Expression Compare()
        {
           
            Expression x = Expr();
            while (look.tag == '>' || look.tag == '<' || look.tag == Tag.LT || look.tag == Tag.GT)
            {
                Token tok = look;
                Move();
                x = new BinaryExpression(tok, x, Expr());
            }

           


            return x;


        }


        /// <summary>
        /// 语句
        /// Expr  -> Term Expr'
        /// Expr' -> + Term Expr'
        ///          | - Term Expr'
        ///          | ε
        /// </summary>
        /// <returns></returns>
        Expression Expr()
        {
            Expression x = Term();
            while (look.tag == '+' || look.tag == '-')
            {
                Token tok = look;
                Move();
                x = new BinaryExpression(tok, x, Term());
            }
            return x;
        }


        /// <summary>
        /// 语句
        /// Term  -> Unary Term'
        /// Term' -> * Unary Term'
        ///          | / Unary Term
        ///          | % Unary Term'
        ///          | ε
        /// </summary>
        /// <returns></returns>
        Expression Term()
        {
            Expression x = Unary();
            while (look.tag == '*' || look.tag == '/' || look.tag == '%')
            {
                Token tok = look;
                Move();
                x = new BinaryExpression(tok, x, Unary());
            }
            return x;
        }

      

        /// <summary>
        /// 语句
        /// Unary -> !  Unary
        ///          | -  Unary
        ///          | Factor
        /// </summary>
        /// <returns></returns>
        Expression Unary()
        {
            if (look.tag == '!' || look.tag == '-')
            {
                Token t = look;
                Move();
                Expression e = Factor();
                UnaryExpression ue = new UnaryExpression(look, e);

                return ue;
            }
            else
            {
                return Factor();
            }
        }


        /// <summary>
        /// 该函数用于处理因子
        /// </summary>
        /// <returns></returns>
        Expression Factor()
        {
            Expression x = null;
            if(look.tag == '(')
            {

                Move();
                x = Bool();
                Match(')');
                return x;
            }
            else if(look.tag == Tag.NUM || look.tag == Tag.REAL || look.tag == Tag.TRUE || look.tag == Tag.FALSE)
            {
                
                Num n = look as Num;
                Literal l = new Literal("" + n.value, n.value);
                Move();
                return l;
            }
            else if(look.tag == Tag.INC) // ++i
            {
                Token tok = look;
                Move();
                Word id = look as Word;
                Identifier identifier = new Identifier("int", id.lexeme);
                UpdateExpression updateExpression = new UpdateExpression(tok, identifier, true);
                return updateExpression;
            }
            else if (look.tag == Tag.ID)
            {
                Word id = look as Word;

                Identifier identifier = new Identifier("int", id.lexeme);
                Move();
                if(look.tag == '(')
                {
                    Match('(');
                    // 函数调用作为因子
                   
                    CallExpression callExpression = new CallExpression(new Identifier("int", id.lexeme), Argumentlist());
                    Match(')');
                    Match(';');
                    return callExpression;
                    

                }
                else if(look.tag == Tag.INC)    // i++
                {

                    Token tok = look;
                    Move();
                    UpdateExpression updateExpression = new UpdateExpression(tok, identifier, false);
                    return updateExpression;
                }
                else
                {
                    return identifier;
                }

                
            }
            else 
            {
                return x;
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
                //Id id = new Id((Word)tok, p, used);
                // 将标识符及标识符对应的类型信息存入到符号表中
              //  top.Put(tok, id);
                used = used + p.width;

                if(look.tag == ',')
                {
                    while (look.tag == ',')
                    {
                        Match(',');
                        Token t = look; // 获取标识符
                  //      Id eid = new Id((Word)t, p, used);
                        Match(Tag.ID);
                 //       top.Put(t, eid);
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
                 //   Id id = new Id((Word)t, p, used);
                    Match(Tag.ID);
              //      top.Put(t, id);
                }
                Match(';');
            }
            else if(look.tag == ';')
            {
             //   Id id = new Id((Word)tok, p, used);
           //     top.Put(tok, id);

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



        /*void Join()
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
