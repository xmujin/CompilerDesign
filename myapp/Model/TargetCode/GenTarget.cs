using CefSharp.DevTools.DOM;
using myapp.Model.CodeGen;
using myapp.Model.Inter;
using myapp.Model.Symbols;
using QUT.GplexBuffers;
using System.Text;
namespace myapp.Model.TargetCode
{
    public class GenTarget
    {


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

            sb.Append(GenUserMethod());
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
                    sb.AppendLine($"\t{symbol.name}\tdd\t0");
                }
            }

            sb.AppendLine("data ends");
            return sb.ToString();
        }

        /// <summary>
        /// 用户定义的函数
        /// </summary>
        /// <returns></returns>
        public string GenUserMethod()
        {
            StringBuilder stringBuilder = new StringBuilder();
            return stringBuilder.ToString();
        }


        /// <summary>
        /// 系统定义的函数
        /// </summary>
        /// <returns></returns>
        public string GenOtherMethod()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("    read proc    \r\n        ; 读取用户输入\r\n        mov ah, 0Ah\r\n        lea dx, num_buffer\r\n        int 21h\r\n    \r\n        ; 获取输入长度\r\n        mov si, offset num_buffer\r\n        xor cx, cx\r\n        mov cl, [si + 1]  ; 获取字符个数\r\n        ;dec cx\r\n        add si, 2  ; 跳过缓冲区两个字节\r\n        xor ax, ax  \r\n        xor dx, dx\r\n    get:\r\n        mov dx, 10      ; ax = ax * 10 + ax\r\n        mul dx\r\n        mov dl, [si]  \r\n        sub dl, '0' \r\n        add al, dl\r\n        inc si\r\n        loop get\r\n        ret\r\n    read endp\r\n\r\n        write proc\r\n        lea di, output_buffer + 15  \r\n        mov byte ptr [di], '$'  \r\n        dec di\r\n\r\n    convert_to_string:\r\n        mov dx, 0        \r\n        mov cx, 10       \r\n        div cx          \r\n        add dl, '0'      \r\n        mov [di], dl     \r\n        dec di\r\n        test ax, ax      \r\n        jnz convert_to_string  \r\n        inc di           \r\n\r\n    print_result:\r\n        mov dx, offset output_message\r\n        mov ah, 09h\r\n        int 21h\r\n\r\n        ; 打印字符串\r\n        mov dx, di\r\n        mov ah, 09h\r\n        int 21h\r\n\r\n        ; 添加换行以清晰显示输出\r\n        mov dx, offset newline\r\n        mov ah, 09h\r\n        int 21h\r\n        ret\r\n    write endp");
            return stringBuilder.ToString();
        }


    }
}
