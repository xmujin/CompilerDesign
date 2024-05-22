using myapp.Model.Inter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace myapp.Model.Symbols
{
    

    public class OldSymbolTable
    {
        private Stack<Scope> scopeStack;
        private List<Scope> scopeHistory;

        public SymbolTable()
        {
            scopeStack = new Stack<Scope>();
            scopeHistory = new List<Scope>();
            EnterScope("global"); // 进入全局作用域
        }

        // 进入一个新的作用域
        public void EnterScope(string scopeName)
        {
            Scope newScope = new Scope(scopeName);
            scopeStack.Push(newScope);
        }

        // 离开当前作用域并记录符号信息
        public void ExitScope()
        {
            if (scopeStack.Count > 0)
            {
                Scope exitingScope = scopeStack.Pop();
                scopeHistory.Add(exitingScope);
            }
            else
            {
                throw new InvalidOperationException("Cannot exit from global scope.");
            }
        }

        // 获取当前作用域名
        public string GetCurrentScopeName()
        {
            return scopeStack.Count > 0 ? scopeStack.Peek().Name : "";
        }

        // 添加符号到当前作用域
        public void AddSymbol(string name, Node node)
        {
            if (scopeStack.Count > 0)
            {
                Scope currentScope = scopeStack.Peek();
                currentScope.AddSymbol(name, node);
            }
            else
            {
                throw new InvalidOperationException("No scope available.");
            }
        }

        // 查找符号
        public Node LookupSymbol(string name)
        {
            foreach (var scope in scopeStack)
            {
                var symbol = scope.LookupSymbol(name);
                if (symbol != null)
                {
                    return symbol;
                }
            }
            return null;
        }

        // 打印当前作用域的符号表
        public void PrintCurrentScope()
        {
            if (scopeStack.Count > 0)
            {
                Scope currentScope = scopeStack.Peek();
                //Console.WriteLine($"Current Scope: {currentScope.Name}");
                currentScope.PrintSymbols();
            }
            else
            {
                Console.WriteLine("No current scope.");
            }
        }

        // 打印所有作用域的历史记录
        public void PrintScopeHistory()
        {
            
            foreach (var scope in scopeHistory)
            {
                if(scope.symbols.Count > 0)
                {
                    scope.PrintSymbols();
                }
                


            }
        }
    }

    /// <summary>
    /// 作用域，包含了符号信息
    /// </summary>
    public class Scope
    {
        public string Name { get; }
        public Dictionary<string, Node> symbols;

        public Scope(string name)
        {
            Name = name;
            symbols = new Dictionary<string, Node>();
        }

        public void AddSymbol(string name, Node node)
        {
            if (!symbols.ContainsKey(name))
            {
                symbols[name] = node;
            }
            else
            {
                throw new ArgumentException($"Symbol '{name}' already exists in scope '{Name}'.");
            }
        }

        public Node LookupSymbol(string name)
        {

            return symbols.ContainsKey(name) ? symbols[name] : null;

        }

        public void PrintSymbols()
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("|{0,-47}|", "变量符号表");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("|{0,-7}|{1,-7}|{2,-7}|{3,-7}|{4,-7}|", "变量名", "长度", "类型", "初始值", "作用域");
            Console.WriteLine("--------------------------------------------------------");
            foreach (var symbol in symbols.Values)
            {
                if(symbol is VariableDeclarator variable)
                {
                    Console.WriteLine("|{0,-7}|{1,-7}|{2,-7}|{3,-7}|{4,-7}|", variable.id, 4, variable.id.idtype, "初始值", Name);
                    Console.WriteLine("--------------------------------------------------------");

                }
                
            }
            Console.WriteLine("------------------------");
            Console.WriteLine("|{0,-19}|", "函数表");
            Console.WriteLine("------------------------");
            Console.WriteLine("|{0,-7}|{1,-7}|", "函数名", "返回类型");
            Console.WriteLine("------------------------");
            foreach (var symbol in symbols.Values)
            {
                if (symbol is FunctionDeclaration function)
                {
                    Console.WriteLine("|{0,-10}|{1,-11}|", function.id, function.id.idtype);
                }

            }

            Console.WriteLine("------------------------");



        }
    }



    /// <summary>
    /// 符号表信息
    /// </summary>
    public class SymbolInfo
    {

        /// <summary>
        /// 符号名  
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 符号表 类型
        /// </summary>
        public string Type { get; }

        public SymbolInfo(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }

    public class VarSymbolInfo : SymbolInfo
    {


        public VarSymbolInfo(string name, string type) : base(name, type)
        {



        }
    }

    /// <summary>
    /// 常量的符号
    /// </summary>
    public class ConsSymbolInfo : SymbolInfo
    {

        public string Value { get; }
        public ConsSymbolInfo(string name, string type, string value) : base(name, type)
        {
            Value = value;
        }
    }


}
