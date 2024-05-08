using myapp.Model.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = myapp.Model.Symbols.Type;

namespace myapp.Model.Inter
{
    public class Function : Token
    {
        List<Id> paramlist;
        Stmt block;
        Type returntype;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returntype">函数返回类型</param>
        /// <param name="paramlist">函数的参数列表</param>
        /// <param name="block">函数体</param>
        public Function(Type returntype, List<Id> paramlist, Stmt block) : base(Tag.FUNC)
        {
            this.returntype = returntype;
            this.paramlist = paramlist;
            this.block = block;
        }
    }
}
