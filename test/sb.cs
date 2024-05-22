using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
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

    public class SymbolTable
    {
        private readonly Dictionary<string, Symbol> _symbols;
        public SymbolTable Parent { get; }
        private readonly List<SymbolTable> _children;

        public SymbolTable(SymbolTable parent = null)
        {
            _symbols = new Dictionary<string, Symbol>();
            Parent = parent;
            _children = new List<SymbolTable>();

            // If this is not the root (global) scope, add this to the parent's children
            Parent?.AddChild(this);
        }

        public bool AddSymbol(Symbol symbol)
        {
            if (_symbols.ContainsKey(symbol.Name))
            {
                return false; // Symbol already exists in the current scope
            }
            _symbols[symbol.Name] = symbol;
            return true;
        }

        public Symbol Lookup(string name)
        {
            if (_symbols.TryGetValue(name, out var symbol))
            {
                return symbol;
            }
            return Parent?.Lookup(name); // Recursive lookup in parent scopes
        }

        private void AddChild(SymbolTable child)
        {
            _children.Add(child);
        }

        public void PrintSymbols(int level = 0)
        {
            foreach (var symbol in _symbols.Values)
            {
                Console.WriteLine($"{new string(' ', level * 2)}Level {level}: {symbol}");
            }
            foreach (var child in _children)
            {
                child.PrintSymbols(level + 1);
            }
        }
    }

    public class Compiler
    {
        private SymbolTable _currentScope;
        private readonly SymbolTable _globalScope;

        public Compiler()
        {
            _globalScope = new SymbolTable();
            _currentScope = _globalScope;
        }

        public void DefineVariable(string name, string type)
        {
            var variable = new VariableSymbol(name, type);
            if (!_currentScope.AddSymbol(variable))
            {
                throw new Exception($"Variable {name} already defined in the current scope.");
            }
        }

        public void DefineFunction(string name, string returnType, List<Symbol> parameters)
        {
            var function = new FunctionSymbol(name, returnType, parameters);
            if (!_currentScope.AddSymbol(function))
            {
                throw new Exception($"Function {name} already defined in the current scope.");
            }
        }

        public Symbol Lookup(string name)
        {
            var symbol = _currentScope.Lookup(name);
            if (symbol == null)
            {
                throw new Exception($"Symbol {name} not found.");
            }
            return symbol;
        }

        public void EnterScope()
        {
            _currentScope = new SymbolTable(_currentScope);
        }

        public void ExitScope()
        {
            if (_currentScope.Parent != null)
            {
                _currentScope = _currentScope.Parent;
            }
            else
            {
                throw new InvalidOperationException("Cannot exit the global scope.");
            }
        }

        public void PrintSymbolTable()
        {
            _globalScope.PrintSymbols();
        }
    }




}
