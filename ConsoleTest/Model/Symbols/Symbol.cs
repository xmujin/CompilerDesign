using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Symbols
{
    public class Symbol
    {
        public string name;

        public string type;

        public Symbol(string name, string type)
        {
            this.name = name;
            this.type = type;
        }
    }


    /// <summary>
    /// 变量名, 长度, 类型, 初始值, 作用域
    /// </summary>
    public class VariableSymbol : Symbol
    {
        int size;
        string init;
        string scope;
        public VariableSymbol(string name, string type, string init, string scope) : base(name, type)
        {
            this.init = init;
            this.scope = scope;
        }
    }

    /// <summary>
    ///  函数名, 返回类型
    /// </summary>
    public class FunctionSymbol : Symbol
    {

        public FunctionSymbol(string name, string type) : base(name, type)
        {

        }

    }




}
