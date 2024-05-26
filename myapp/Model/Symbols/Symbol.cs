using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public int size;
        public string init;
        public string scope;
        public int offset;
        public bool isUsed = false;
        public VariableSymbol(string name, int size,string type, string init, string scope) : base(name, type)
        {
            this.size = size;
            this.init = init;
            this.scope = scope;
        }

        public override string ToString() => $"{name} : {type}, {init}, {scope})";
    }

    /// <summary>
    ///  函数名, 返回类型
    /// </summary>
    public class FunctionSymbol : Symbol
    {

        public FunctionSymbol(string name, string type) : base(name, type)
        {

        }
        public override string ToString() => $"函数名：{name} ， 返回类型： {type}";
    }




}
