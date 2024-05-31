using CefSharp.DevTools.DOM;
using myapp.Model.CodeGen;
using myapp.Model.Inter;
using myapp.Model.Lexer;
using myapp.Model.Symbols;
using QUT.GplexBuffers;
using System.Text;
namespace myapp.Model.TargetCode
{
    public class GenTarget
    {

        int offset = 0;
        List<Quadruple> quadruples;

        public GenTarget(List<Quadruple> quadruples) 
        {
            this.quadruples = quadruples;
        }

        public string GenAll(SymbolTableManager symbolTabelManager)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GenCodeInit());
            sb.Append(GenVariableDeclaration(symbolTabelManager));


            sb.AppendLine("code segment");

            sb.Append(GenUserMethod(symbolTabelManager));
            sb.Append(GenOtherMethod());
            sb.AppendLine("code ends");
            sb.AppendLine("end main");
            return sb.ToString();
        }



        public string GenCodeInit()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("assume cs:code, ds:data");

            // 定义栈段
            sb.AppendLine("stack segment stack");
            sb.AppendLine("\tdb 256 dup(0)");  // 定义 256 字节的栈段并初始化为 0
            sb.AppendLine("stack ends");


            return sb.ToString();
        }




        /// <summary>
        /// 汇编全局变量的声明，全局变量放在数据段，而局部变量存放在栈上
        /// </summary>
        /// <param name="symbolTabelManager"></param>
        /// <returns></returns>
        public string GenVariableDeclaration(SymbolTableManager symbolTabelManager)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("data segment");
            
            foreach (var symbol in symbolTabelManager._globalScope._symbols.Values)
            {
                if (symbol is VariableSymbol vs)
                {
                    sb.AppendLine($"\t{vs.name}_{vs.scope}\tdw\t{vs.init}"); // 定义全局变量
                }
            }
            sb.AppendLine("    message db 'Enter a number(-_-): ',0dh, 0ah, '$'\r\n    num_buffer db 16\r\n               db 15 dup(0)\r\n    newline db 0dh, 0ah, '$'\r\n    output_buffer db 16 dup(0)  ; 用于存储转换后的数字字符串 (最多5位数+1个终止符)\r\n    output_message db 'The number is: $'");
            sb.AppendLine("data ends");


            return sb.ToString();
        }

        /// <summary>
        /// 用户定义的函数
        /// </summary>
        /// <returns></returns>
        public string GenUserMethod(SymbolTableManager symbolTabelManager)
        {
            StringBuilder stringBuilder = new StringBuilder();


            foreach(Quadruple quadruple in quadruples)
            {
                if(quadruple.op == "func")
                {

                    if(quadruple.arg2 == "begin")
                    {
                        stringBuilder.AppendLine($"\t{quadruple.arg1} proc");
                        // 对于其他函数   需要分配栈
                        if(quadruple.arg1 != "main")
                        {

                        }
                        else
                        {
                            stringBuilder.AppendLine("\tmov ax, data\r\n        mov ds, ax\r\n        \r\n        ; 初始化栈段寄存器和栈指针\r\n        mov ax, stack\r\n        mov ss, ax\r\n        mov sp, 256 ; 设置栈指针到栈顶");
                            stringBuilder.AppendLine("\tmov bp, sp");
                        }



                    }
                    else
                    {
                        if (quadruple.arg1 == "main")
                        {
                            stringBuilder.AppendLine($"\t        ; 退出程序\r\n        mov AX, 4C00h\r\n        int 21h");
                        }
                        stringBuilder.AppendLine($"\t{quadruple.arg1} endp");
                    }



                }
                else if(quadruple.op == "call") // 函数调用统一将返回值存放在ax中
                {
                    stringBuilder.AppendLine($"\tcall " + quadruple.arg1);
                }
                else if(quadruple.op == "=") //赋值语句
                {

                    string id = quadruple.result;    // 获取变量名   以name_scope 形式存在
                    int lastIndex = id.LastIndexOf('_');// 以最后一个下划线分隔
                    string name = id.Substring(0, lastIndex);
                    string scope = id.Substring(lastIndex + 1);
                    VariableSymbol vs = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name, scope);


                    bool judge = int.TryParse(quadruple.arg1, out int num);

                    // 变量未使用   进行栈分配
                    if (vs.scope != "0" && vs.isUsed == false)
                    {
                        vs.isUsed = true;
                        //stringBuilder.AppendLine($"\tpop ax"); // 使用ax寄存器
                        //stringBuilder.AppendLine("\tmov bp, sp"); // 保存栈基地址
                        


                        if(quadruple.arg1 == "result" || quadruple.arg1.StartsWith('#')) // 以t开头或名为result都为临时变量，存放与ax
                        {
                            offset += 2;
                            vs.offset = offset;
                            if(quadruple.arg1 != "result")
                                stringBuilder.AppendLine($"\tpop ax"); // 使用ax寄存器
                            stringBuilder.AppendLine("\tsub sp, 2"); // 分配2字节空间
                            stringBuilder.AppendLine($"\tmov word ptr [bp - {vs.offset}], ax"); // 使用ax寄存器
                        }
                        else if(judge)    // 当arg1 为数字时  直接赋值
                        {
                            offset += 2;
                            vs.offset = offset;
                            stringBuilder.AppendLine($"\tmov ax, {num}"); // 使用ax寄存器
                            stringBuilder.AppendLine("\tsub sp, 2"); // 分配2字节空间
                            stringBuilder.AppendLine($"\tmov word ptr [bp - {vs.offset}], ax"); // 使用ax寄存器
                        }
                        else // 两个普通变量赋值
                        {
                            string name1 = quadruple.arg1.Substring(0, quadruple.arg1.LastIndexOf('_'));
                            string scope1 = quadruple.arg1.Substring(quadruple.arg1.LastIndexOf('_') + 1);
                            VariableSymbol vs1 = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name1, scope1);
                            offset += 2;
                            vs.offset = offset;
                            stringBuilder.AppendLine($"\tmov ax, [bp - {vs1.offset}]"); // 使用ax寄存器
                            stringBuilder.AppendLine("\tsub sp, 2"); // 分配2字节空间
                            stringBuilder.AppendLine($"\tmov word ptr [bp - {vs.offset}], ax"); // 使用ax寄存器
                        }



                            
                    }
                    else if(vs.scope != "0" && vs.isUsed == true && IsTemp(quadruple.arg1)) 
                    {


                        stringBuilder.AppendLine($"\tpop ax"); // 使用ax寄存器
                        stringBuilder.AppendLine($"\tmov word ptr [bp - {vs.offset}], ax"); // 使用ax寄存器


                    }
                    else if(vs.scope != "0" && vs.isUsed == true && IsVariable(quadruple.arg1))    // 赋值为变量的情况
                    {
                        string name1 = quadruple.arg1.Substring(0, quadruple.arg1.LastIndexOf('_'));
                        string scope1 = quadruple.arg1.Substring(quadruple.arg1.LastIndexOf('_') + 1);
                        VariableSymbol vs1 = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name1, scope1);
                        stringBuilder.AppendLine($"\tmov ax, [bp - {vs1.offset}]"); // 使用ax寄存器
                        stringBuilder.AppendLine($"\tmov  [bp - {vs.offset}], ax"); // 使用ax寄存器
                    }
                    else if (vs.scope != "0" && vs.isUsed == true && quadruple.arg1 == "result")    // 赋值为变量的情况
                    {
                        stringBuilder.AppendLine($"\tpop ax"); // 使用ax寄存器
                        stringBuilder.AppendLine($"\tmov  [bp - {vs.offset}], ax"); // 使用ax寄存器
                    }

                    else if(vs.scope != "0" && vs.isUsed == true && int.TryParse(quadruple.arg1, out int n)) // 赋值为数字的情况
                    {
                        stringBuilder.AppendLine($"\tmov ax, {num}"); // 使用ax寄存器
                        stringBuilder.AppendLine($"\tmov word ptr [bp - {vs.offset}], ax"); // 使用ax寄存器
                    }

                    else if(vs.scope == "0" && IsTemp(quadruple.arg1))
                    {

                        stringBuilder.AppendLine($"\tpop ax"); // 使用ax寄存器
                        stringBuilder.AppendLine($"\tmov  {vs.name}_{vs.scope}, ax"); // 使用ax寄存器

                    }



                }
                else if(quadruple.op == "param")
                {
                    // 将参数进行入栈
                    string id = quadruple.arg1;    // 获取变量名   以name_scope 形式存在
                    int lastIndex = id.LastIndexOf('_');// 以最后一个下划线分隔
                    string name = id.Substring(0, lastIndex);
                    string scope = id.Substring(lastIndex + 1);
                    VariableSymbol vs = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name, scope);
                    if(vs.scope != "0")
                        stringBuilder.AppendLine($"\tpush [bp - {vs.offset}]"); // 将参数进行入栈
                    else
                        stringBuilder.AppendLine($"\tpush {vs.name}_{vs.scope}"); // 将参数进行入栈


                }
                else if(quadruple.op == "+" || quadruple.op == "-")
                {

                    bool left = int.TryParse(quadruple.arg1, out int a);
                    bool right = int.TryParse(quadruple.arg2, out int b);
                    if (left && right) // 如果四元式都是数字
                    {
                        if (quadruple.op == "+")
                        {
                            stringBuilder.AppendLine($"\tmov ax, {a}");
                            stringBuilder.AppendLine($"\tadd ax, {b}");
                        }
                        else
                        {
                            stringBuilder.AppendLine($"\tmov ax, {a}");
                            stringBuilder.AppendLine($"\tsub ax, {b}");
                        }

                        stringBuilder.AppendLine($"\tpush ax");

                    }
                    else if(left && !right) // 左数字  右边变量
                    {

                        if (quadruple.arg2.StartsWith('#'))  // 临时变量
                        {
                            
                            if (quadruple.op == "+")
                            {
                                stringBuilder.AppendLine($"\tpop ax");
                                stringBuilder.AppendLine($"\tadd ax, {a}");
                            }
                            else
                            {
                                stringBuilder.AppendLine($"\tpop dx");
                                stringBuilder.AppendLine($"\tmov ax, {a}");
                                stringBuilder.AppendLine($"\tsub ax, dx");
                            }
                            stringBuilder.AppendLine($"\tpush ax");
                        }
                        else // 非临时变量的情况
                        {

                            string id = quadruple.arg2;    // 获取变量名   以name_scope 形式存在
                            int lastIndex = id.LastIndexOf('_');// 以最后一个下划线分隔
                            string name = id.Substring(0, lastIndex);
                            string scope = id.Substring(lastIndex + 1);
                            VariableSymbol vs = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name, scope);
                            if(scope != "0")
                                stringBuilder.AppendLine($"\tmov ax, [bp - {vs.offset}]");
                            else
                                stringBuilder.AppendLine($"\tmov ax, {vs.name}_{vs.scope}");
                            if (quadruple.op == "+")
                            {
                                stringBuilder.AppendLine($"\tadd ax, {a}");
                            }
                            else
                            {
                                stringBuilder.AppendLine($"\tsub ax, {a}");
                            }
                            stringBuilder.AppendLine($"\tpush ax");
                        }

                        
                    }
                    else if (!left && right) // 左变量  右边数字
                    {
                        if(quadruple.arg1.StartsWith('#'))
                        {
                            if (quadruple.op == "+")
                            {
                                stringBuilder.AppendLine($"\tpop ax");
                                stringBuilder.AppendLine($"\tadd ax, {b}");
                                stringBuilder.AppendLine($"\tpush ax");
                            }
                            else
                            {
                                stringBuilder.AppendLine($"\tpop ax");
                                stringBuilder.AppendLine($"\tsub ax, {b}");
                                stringBuilder.AppendLine($"\tpush ax");
                            }
                        }
                        else
                        {

                            string id = quadruple.arg1;    // 获取变量名   以name_scope 形式存在
                            int lastIndex = id.LastIndexOf('_');// 以最后一个下划线分隔
                            string name = id.Substring(0, lastIndex);
                            string scope = id.Substring(lastIndex + 1);
                            VariableSymbol vs = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name, scope);
                            
                            if (quadruple.op == "+")
                            {
                                if (scope != "0")
                                    stringBuilder.AppendLine($"\tmov ax, [bp - {vs.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov ax, {vs.name}_{vs.scope}");
                                stringBuilder.AppendLine($"\tadd ax, {b}");
                                stringBuilder.AppendLine($"\tpush ax");
                            }
                            else
                            {
                                if (scope != "0")
                                    stringBuilder.AppendLine($"\tmov ax, [bp - {vs.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov ax, {vs.name}_{vs.scope}");
                                stringBuilder.AppendLine($"\tsub ax, {b}");
                                stringBuilder.AppendLine($"\tpush ax");
                            }
                        }
                        
                        
                    }
                    else  // 两边都为临时变量的情况
                    {

                        if (quadruple.arg1.StartsWith('#') && quadruple.arg2.StartsWith('#'))// 左临时  右临时
                        {
                            // first    right pop
                            stringBuilder.AppendLine($"\tpop dx");
                            stringBuilder.AppendLine($"\tpop ax");
                            if (quadruple.op == "+")
                            {
                                stringBuilder.AppendLine($"\tadd ax, dx");
                            }
                            else
                            {
                                stringBuilder.AppendLine($"\tsub ax, dx");
                            }
                            stringBuilder.AppendLine($"\tpush ax");
                        }
                        else if(quadruple.arg1.StartsWith('#') && !quadruple.arg2.StartsWith('#')) // 左临时  右变量
                        {
                            string id = quadruple.arg2;    // 获取变量名   以name_scope 形式存在
                            int lastIndex = id.LastIndexOf('_');// 以最后一个下划线分隔
                            string name = id.Substring(0, lastIndex);
                            string scope = id.Substring(lastIndex + 1);
                            VariableSymbol vs = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name, scope);

                            
                            if (quadruple.op == "+")
                            {
                                stringBuilder.AppendLine($"\tpop ax");
                                if (scope != "0")
                                    stringBuilder.AppendLine($"\tmov dx, [bp - {vs.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov dx, {vs.name}_{vs.scope}");
                                stringBuilder.AppendLine($"\tadd ax, dx");

                            }
                            else
                            {
                                stringBuilder.AppendLine($"\tpop ax");
                                if (scope != "0")
                                    stringBuilder.AppendLine($"\tmov dx, [bp - {vs.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov dx, {vs.name}_{vs.scope}");
                                stringBuilder.AppendLine($"\tsub ax, dx");
                            }
                            stringBuilder.AppendLine($"\tpush ax");


                        }
                        else if (!quadruple.arg1.StartsWith('#') && quadruple.arg2.StartsWith('#')) // 左变量  右临时
                        {
                            string id = quadruple.arg1;    // 获取变量名   以name_scope 形式存在
                            int lastIndex = id.LastIndexOf('_');// 以最后一个下划线分隔
                            string name = id.Substring(0, lastIndex);
                            string scope = id.Substring(lastIndex + 1);
                            VariableSymbol vs = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name, scope);
                            if (quadruple.op == "+")
                            {
                                stringBuilder.AppendLine($"\tpop dx");
                                if (scope != "0")
                                    stringBuilder.AppendLine($"\tmov ax, [bp - {vs.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov ax, {vs.name}_{vs.scope}");
                                stringBuilder.AppendLine($"\tadd ax, dx");

                            }
                            else
                            {
                                stringBuilder.AppendLine($"\tpop dx");
                                if (scope != "0")
                                    stringBuilder.AppendLine($"\tmov ax, [bp - {vs.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov ax, {vs.name}_{vs.scope}");
                                stringBuilder.AppendLine($"\tsub ax, dx");
                            }
                            stringBuilder.AppendLine($"\tpush ax");
                        }
                        else // 左变量  右变量
                        {
                            string id1 = quadruple.arg1;    // 获取变量名   以name_scope 形式存在
                            int lastIndex1 = id1.LastIndexOf('_');// 以最后一个下划线分隔
                            string name1 = id1.Substring(0, lastIndex1);
                            string scope1 = id1.Substring(lastIndex1 + 1);
                            VariableSymbol vs1 = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name1, scope1);

                            string id2 = quadruple.arg2;    // 获取变量名   以name_scope 形式存在
                            int lastIndex2 = id2.LastIndexOf('_');// 以最后一个下划线分隔
                            string name2 = id2.Substring(0, lastIndex2);
                            string scope2 = id2.Substring(lastIndex2 + 1);
                            VariableSymbol vs2 = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name2, scope2);


                            if (quadruple.op == "+")
                            {
                                if (scope1 != "0")
                                    stringBuilder.AppendLine($"\tmov ax, [bp - {vs1.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov ax, {vs1.name}_{vs1.scope}");

                                if (scope2 != "0")
                                    stringBuilder.AppendLine($"\tmov dx, [bp - {vs2.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov dx, {vs2.name}_{vs2.scope}");
                                stringBuilder.AppendLine($"\tadd ax, dx");

                            }
                            else
                            {
                                if (scope1 != "0")
                                    stringBuilder.AppendLine($"\tmov ax, [bp - {vs1.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov ax, {vs1.name}_{vs1.scope}");

                                if (scope2 != "0")
                                    stringBuilder.AppendLine($"\tmov dx, [bp - {vs2.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov dx, {vs2.name}_{vs2.scope}");
                                stringBuilder.AppendLine($"\tsub ax, dx");
                            }


                            stringBuilder.AppendLine($"\tpush ax");

                        }
                    }
                }
                else if(quadruple.op == "*" || quadruple.op == "/" || quadruple.op == "%")
                {

                    bool left = int.TryParse(quadruple.arg1, out int a);
                    bool right = int.TryParse(quadruple.arg2, out int b);
                    if (left && right) // 如果四元式都是数字
                    {
                        if (quadruple.op == "*")
                        {
                            stringBuilder.AppendLine($"\tmov ax, {a}");
                            stringBuilder.AppendLine($"\tmov dx, {b}");



                            stringBuilder.AppendLine($"\tmul dx");
                        }
                        else
                        {
                            stringBuilder.AppendLine($"\tmov ax, {a}"); // div  商存放在al寄存器中，余数存放在ah中
                            stringBuilder.AppendLine($"\tmov dl, {b}");
                            stringBuilder.AppendLine($"\tdiv dl");
                            if(quadruple.op == "%")
                            {
                                stringBuilder.AppendLine($"\txor al, al");  //  清除商
                                // 将余数右移
                                stringBuilder.AppendLine($"\tshr ax, 8");
                            }
                            else   stringBuilder.AppendLine($"\txor ah, ah");  // 清除余数

                        }

                        stringBuilder.AppendLine($"\tpush ax");

                    }
                    else if (left && !right) // 左数字  右边变量
                    {


                        if (quadruple.arg2.StartsWith('#'))  // 临时变量
                        {
                            if (quadruple.op == "*")
                            {
                                stringBuilder.AppendLine($"\tpop dx");  // 将结果放在dx 中 再乘
                                stringBuilder.AppendLine($"\tmov ax, {a}");
                                stringBuilder.AppendLine($"\tmul dx");
                            }
                            else
                            {
                                stringBuilder.AppendLine($"\tpop dx");  // 将结果放在dx 中 再乘
                                stringBuilder.AppendLine($"\tmov ax, {a}"); // div  商存放在al寄存器中，余数存放在ah中
                                stringBuilder.AppendLine($"\tdiv dl");
                                if (quadruple.op == "%")
                                {
                                    stringBuilder.AppendLine($"\txor al, al");  //  清除商
                                                                                // 将余数右移
                                    stringBuilder.AppendLine($"\tshr ax, 8");
                                }
                                else stringBuilder.AppendLine($"\txor ah, ah");  // 清除余数
                            }
                            stringBuilder.AppendLine($"\tpush ax");
                        }
                        else // 参数2不是临时变量
                        {
                            string id2 = quadruple.arg2;    // 获取变量名   以name_scope 形式存在
                            int lastIndex2 = id2.LastIndexOf('_');// 以最后一个下划线分隔
                            string name2 = id2.Substring(0, lastIndex2);
                            string scope2 = id2.Substring(lastIndex2 + 1);
                            VariableSymbol vs2 = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name2, scope2);
                            if (scope2 != "0")
                                stringBuilder.AppendLine($"\tmov ax, [bp - {vs2.offset}]");
                            else
                                stringBuilder.AppendLine($"\tmov ax, {vs2.name}_{vs2.scope}");

                            if (quadruple.op == "*")
                            {
                                stringBuilder.AppendLine($"\tmov ax, {a}");
                                stringBuilder.AppendLine($"\tmul dx");
                            }
                            else
                            {
                                stringBuilder.AppendLine($"\tmov ax, {a}");
                                stringBuilder.AppendLine($"\tdiv dl");
                                if (quadruple.op == "%")
                                {
                                    stringBuilder.AppendLine($"\txor al, al");  //  清除商
                                                                                // 将余数右移
                                    stringBuilder.AppendLine($"\tshr ax, 8");
                                }
                                else stringBuilder.AppendLine($"\txor ah, ah");  // 清除余数
                            }
                            stringBuilder.AppendLine($"\tpush ax");
                        }
                    }
                    else if (!left && right) // 左变量  右边数字
                    {
                        if (quadruple.arg1.StartsWith('#'))  // 临时变量
                        {
                            if (quadruple.op == "*")
                            {
                                stringBuilder.AppendLine($"\tpop dx");  // 将结果放在dx 中 再乘
                                stringBuilder.AppendLine($"\tmov ax, {a}");
                                stringBuilder.AppendLine($"\tmul dx");
                            }
                            else
                            {
                                stringBuilder.AppendLine($"\tpop dx");  // 将结果放在dx 中 再乘
                                stringBuilder.AppendLine($"\tmov ax, {a}"); // div  商存放在al寄存器中，余数存放在ah中
                                stringBuilder.AppendLine($"\tdiv dl");
                                if (quadruple.op == "%")
                                {
                                    stringBuilder.AppendLine($"\txor al, al");  //  清除商
                                                                                // 将余数右移
                                    stringBuilder.AppendLine($"\tshr ax, 8");
                                }
                                else stringBuilder.AppendLine($"\txor ah, ah");  // 清除余数
                            }
                            stringBuilder.AppendLine($"\tpush ax");
                        }
                        else  // 左为普通变量
                        {
                            string id = quadruple.arg1;    // 获取变量名   以name_scope 形式存在
                            int lastIndex = id.LastIndexOf('_');// 以最后一个下划线分隔
                            string name = id.Substring(0, lastIndex);
                            string scope = id.Substring(lastIndex + 1);
                            VariableSymbol vs = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name, scope);
                            if (scope != "0")
                                stringBuilder.AppendLine($"\tmov ax, [bp - {vs.offset}]");
                            else
                                stringBuilder.AppendLine($"\tmov ax, {vs.name}_{vs.scope}");
                            if (quadruple.op == "*")
                            {

                                stringBuilder.AppendLine($"\tmov dx, {b}");
                                stringBuilder.AppendLine($"\tmul dx");
                            }
                            else
                            {
                                stringBuilder.AppendLine($"\tmov dx, {b}");
                                stringBuilder.AppendLine($"\tdiv dl");
                                if (quadruple.op == "%")
                                {
                                    stringBuilder.AppendLine($"\txor al, al");  //  清除商
                                                                                // 将余数右移
                                    stringBuilder.AppendLine($"\tshr ax, 8");
                                }
                                else stringBuilder.AppendLine($"\txor ah, ah");  // 清除余数
                            }
                            stringBuilder.AppendLine($"\tpush ax");
                        }
                    }
                    else  // 两边都为临时变量的情况
                    {
                        if (quadruple.arg1.StartsWith('#') && quadruple.arg2.StartsWith('#'))// 左临时  右临时
                        {
                            // first    right pop
                            stringBuilder.AppendLine($"\tpop dx");
                            stringBuilder.AppendLine($"\tpop ax");
                            if (quadruple.op == "*")
                            {
                                stringBuilder.AppendLine($"\tmul dx");
                            }
                            else
                            {
                                stringBuilder.AppendLine($"\tdiv dl");
                                if (quadruple.op == "%")
                                {
                                    stringBuilder.AppendLine($"\txor al, al");  //  清除商
                                                                                // 将余数右移
                                    stringBuilder.AppendLine($"\tshr ax, 8");
                                }
                                else stringBuilder.AppendLine($"\txor ah, ah");  // 清除余数
                            }
                            stringBuilder.AppendLine($"\tpush ax");
                        }
                        else if (quadruple.arg1.StartsWith('#') && !quadruple.arg2.StartsWith('#')) // 左临时  右变量
                        {
                            string id = quadruple.arg2;    // 获取变量名   以name_scope 形式存在
                            int lastIndex = id.LastIndexOf('_');// 以最后一个下划线分隔
                            string name = id.Substring(0, lastIndex);
                            string scope = id.Substring(lastIndex + 1);
                            VariableSymbol vs = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name, scope);


                            if (quadruple.op == "*")
                            {
                                stringBuilder.AppendLine($"\tpop ax");
                                if (scope != "0")
                                    stringBuilder.AppendLine($"\tmov dx, [bp - {vs.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov dx, {vs.name}_{vs.scope}");
                                stringBuilder.AppendLine($"\tmul dx");

                            }
                            else
                            {
                                stringBuilder.AppendLine($"\tpop ax");
                                if (scope != "0")
                                    stringBuilder.AppendLine($"\tmov dx, [bp - {vs.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov dx, {vs.name}_{vs.scope}");
                                stringBuilder.AppendLine($"\tdiv dl");
                                if (quadruple.op == "%")
                                {
                                    stringBuilder.AppendLine($"\txor al, al");  //  清除商
                                                                                // 将余数右移
                                    stringBuilder.AppendLine($"\tshr ax, 8");
                                }
                                else stringBuilder.AppendLine($"\txor ah, ah");  // 清除余数
                            }
                            stringBuilder.AppendLine($"\tpush ax");


                        }
                        else if (!quadruple.arg1.StartsWith('#') && quadruple.arg2.StartsWith('#')) // 左变量  右临时
                        {
                            string id = quadruple.arg1;    // 获取变量名   以name_scope 形式存在
                            int lastIndex = id.LastIndexOf('_');// 以最后一个下划线分隔
                            string name = id.Substring(0, lastIndex);
                            string scope = id.Substring(lastIndex + 1);
                            VariableSymbol vs = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name, scope);


                            if (quadruple.op == "*")
                            {
                                stringBuilder.AppendLine($"\tpop dx");
                                if (scope != "0")
                                    stringBuilder.AppendLine($"\tmov ax, [bp - {vs.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov ax, {vs.name}_{vs.scope}");
                                stringBuilder.AppendLine($"\tmul dx");

                            }
                            else
                            {
                                stringBuilder.AppendLine($"\tpop dx");
                                if (scope != "0")
                                    stringBuilder.AppendLine($"\tmov ax, [bp - {vs.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov ax, {vs.name}_{vs.scope}");
                                stringBuilder.AppendLine($"\tdiv dl");
                                if (quadruple.op == "%")
                                {
                                    stringBuilder.AppendLine($"\txor al, al");  //  清除商
                                                                                // 将余数右移
                                    stringBuilder.AppendLine($"\tshr ax, 8");
                                }
                                else stringBuilder.AppendLine($"\txor ah, ah");  // 清除余数
                            }
                            stringBuilder.AppendLine($"\tpush ax");
                        }
                        else // 左变量  右变量
                        {
                            string id1 = quadruple.arg1;    // 获取变量名   以name_scope 形式存在
                            int lastIndex1 = id1.LastIndexOf('_');// 以最后一个下划线分隔
                            string name1 = id1.Substring(0, lastIndex1);
                            string scope1 = id1.Substring(lastIndex1 + 1);
                            VariableSymbol vs1 = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name1, scope1);

                            string id2 = quadruple.arg2;    // 获取变量名   以name_scope 形式存在
                            int lastIndex2 = id2.LastIndexOf('_');// 以最后一个下划线分隔
                            string name2 = id2.Substring(0, lastIndex2);
                            string scope2 = id2.Substring(lastIndex2 + 1);
                            VariableSymbol vs2 = (VariableSymbol)symbolTabelManager._globalScope.Lookdown(name2, scope2);


                            if (quadruple.op == "*")
                            {
                                if (scope1 != "0")
                                    stringBuilder.AppendLine($"\tmov ax, [bp - {vs1.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov ax, {vs1.name}_{vs1.scope}");
                                if (scope1 != "0")
                                    stringBuilder.AppendLine($"\tmov dx, [bp - {vs2.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov dx, {vs2.name}_{vs2.scope}");
       
                                stringBuilder.AppendLine($"\tmul dx");

                            }
                            else
                            {
                                if (scope1 != "0")
                                    stringBuilder.AppendLine($"\tmov ax, [bp - {vs1.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov ax, {vs1.name}_{vs1.scope}");
                                if (scope1 != "0")
                                    stringBuilder.AppendLine($"\tmov dx, [bp - {vs2.offset}]");
                                else
                                    stringBuilder.AppendLine($"\tmov dx, {vs2.name}_{vs2.scope}");

                                stringBuilder.AppendLine($"\tdiv dl");
                                if (quadruple.op == "%")
                                {
                                    stringBuilder.AppendLine($"\txor al, al");  //  清除商
                                                                                // 将余数右移
                                    stringBuilder.AppendLine($"\tshr ax, 8");
                                }
                                else stringBuilder.AppendLine($"\txor ah, ah");  // 清除余数
                            }
                            stringBuilder.AppendLine($"\tpush ax");
                        }

                    }
                }
                else if(quadruple.op.StartsWith("j"))// 跳转语句
                {

                    if(quadruple.op == "jmp")
                    {
                        stringBuilder.AppendLine($"\tjmp {quadruple.result}");  
                    }
                    else
                    {
                        if(IsVariable(quadruple.arg1) && IsVariable(quadruple.arg2)) // 两边都是变量
                        {
                            VariableSymbol vs1 = GetVariableSymbol(symbolTabelManager, quadruple.arg1);
                            VariableSymbol vs2 = GetVariableSymbol(symbolTabelManager, quadruple.arg2);

                            if (vs1.scope != "0")
                                stringBuilder.AppendLine($"\tmov ax, [bp - {vs1.offset}]");
                            else
                                stringBuilder.AppendLine($"\tmov ax, {vs1.name}_{vs1.scope}");

                            if (vs2.scope != "0")
                                stringBuilder.AppendLine($"\tcmp ax, [bp - {vs2.offset}]");
                            else
                                stringBuilder.AppendLine($"\tcmp ax, {vs2.name}_{vs2.scope}");


                            stringBuilder.AppendLine($"\t{quadruple.op} {quadruple.result}");
                        }
                        else if(IsVariable(quadruple.arg1)) // 左变量  右边数字
                        {
                            int.TryParse(quadruple.arg2, out int num);
                            VariableSymbol vs = GetVariableSymbol(symbolTabelManager, quadruple.arg1);
                            if (vs.scope != "0")
                                stringBuilder.AppendLine($"\tmov ax, [bp - {vs.offset}]");
                            else
                                stringBuilder.AppendLine($"\tmov ax, {vs.name}_{vs.scope}");

                            stringBuilder.AppendLine($"\tcmp ax, {num}");
                            stringBuilder.AppendLine($"\t{quadruple.op} {quadruple.result}");

                        }
                        else if(IsTemp(quadruple.arg1))
                        {
                            int.TryParse(quadruple.arg2, out int num);
                            stringBuilder.AppendLine($"\tpop ax");
                            stringBuilder.AppendLine($"\tcmp ax, {num}");
                            stringBuilder.AppendLine($"\t{quadruple.op} {quadruple.result}");
                        }
                        else
                        {
                            int.TryParse(quadruple.arg2, out int num1);
                            int.TryParse(quadruple.arg2, out int num2);
                            stringBuilder.AppendLine($"\tmov ax, {num1}");
                            stringBuilder.AppendLine($"\tcmp ax, {num2}");
                            stringBuilder.AppendLine($"\t{quadruple.op} {quadruple.result}");
                        }

                    
                    }
                }
                else if(quadruple.op == "label")
                {
                    stringBuilder.AppendLine($"\t{quadruple.arg1}:");
                }
                else if(quadruple.op == "minus")   // 取负数
                {
                    if(int.TryParse(quadruple.arg1, out int a)) // 是数字的情况
                    {
                        stringBuilder.AppendLine($"\tmov ax, -{quadruple.arg1}");
                        stringBuilder.AppendLine($"\tpush ax");
                    }
                    else if(IsVariable(quadruple.arg1))
                    {
                        VariableSymbol vs = GetVariableSymbol(symbolTabelManager, quadruple.arg1);
                        if (vs.scope != "0")
                            stringBuilder.AppendLine($"\tmov ax, [bp - {vs.offset}]");
                        else
                            stringBuilder.AppendLine($"\tmov ax, {vs.name}_{vs.scope}");

                        stringBuilder.AppendLine($"\tneg ax");

                        if (vs.scope != "0")
                            stringBuilder.AppendLine($"\tmov [bp - {vs.offset}], ax");
                        else
                            stringBuilder.AppendLine($"\tmov {vs.name}_{vs.scope}, ax");



                        stringBuilder.AppendLine($"\tpush ax");
                    }
                }

            }

                


            return stringBuilder.ToString();
        }


        public VariableSymbol GetVariableSymbol(SymbolTableManager stm ,string id)
        {
            int lastIndex1 = id.LastIndexOf('_');// 以最后一个下划线分隔
            string name1 = id.Substring(0, lastIndex1);
            string scope1 = id.Substring(lastIndex1 + 1);
            VariableSymbol vs1 = (VariableSymbol)stm._globalScope.Lookdown(name1, scope1);
            return vs1;
        }

        public bool IsVariable(string v)
        {
            return !v.StartsWith("#") && !int.TryParse( v ,out int res) && v != "result";
        }

        public bool IsTemp(string v) 
        {
            return v.StartsWith("#");
        }

   

        /// <summary>
        /// 系统定义的函数
        /// </summary>
        /// <returns></returns>
        public string GenOtherMethod()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("    read proc    \r\n        ; 读取用户输入\r\n        mov ah, 0Ah\r\n        lea dx, num_buffer\r\n        int 21h\r\n    \r\n        ; 获取输入长度\r\n        mov si, offset num_buffer\r\n        xor cx, cx\r\n        mov cl, [si + 1]  ; 获取字符个数\r\n        ;dec cx\r\n        add si, 2  ; 跳过缓冲区两个字节\r\n        xor ax, ax  \r\n        xor dx, dx\r\n    get:\r\n        mov dx, 10      ; ax = ax * 10 + ax\r\n        mul dx\r\n        mov dl, [si]  \r\n        sub dl, '0' \r\n        add al, dl\r\n        inc si\r\n        loop get\r\n        ret\r\n    read endp\r\n\r\n    write proc\r\n        push bp\r\n        mov bp, sp\r\n        mov ax, [bp + 4]   ; 获取第一个参数, 因为栈空间已经压入了ip，多占用了两字节\r\n        lea di, output_buffer + 15  \r\n        mov byte ptr [di], '$'  \r\n        dec di\r\n\r\n    convert_to_string:\r\n        cmp ax, 0\r\n        jl negative\r\n        mov dx, 0        \r\n        mov cx, 10\r\n        div cx          \r\n        add dl, '0'      \r\n        mov [di], dl     \r\n        dec di\r\n        test ax, ax      \r\n        jnz convert_to_string  \r\n        inc di\r\n        jmp print_result         \r\n    negative:  \r\n        neg ax\r\n    negloop:\r\n        mov dx, 0        \r\n        mov cx, 10\r\n        div cx          \r\n        add dl, '0'      \r\n        mov [di], dl     \r\n        dec di\r\n        test ax, ax      \r\n        jnz negloop\r\n        mov dl, '-'\r\n        mov [di], dl \r\n\r\n    print_result:\r\n        mov dx, offset output_message\r\n        mov ah, 09h\r\n        int 21h\r\n\r\n        ; 打印字符串\r\n        mov dx, di\r\n        mov ah, 09h\r\n        int 21h\r\n\r\n        ; 添加换行以清晰显示输出\r\n        mov dx, offset newline\r\n        mov ah, 09h\r\n        int 21h\r\n\r\n        pop bp\r\n        ret\r\n    write endp");
            return stringBuilder.ToString();
        }


    }
}
