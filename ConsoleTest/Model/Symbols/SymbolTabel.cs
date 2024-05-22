using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Symbols
{

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
            if (_symbols.ContainsKey(symbol.name))
            {
                return false; // Symbol already exists in the current scope
            }
            _symbols[symbol.name] = symbol;
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




        /// <summary>
        /// 打印符号表
        /// </summary>
        /// <param name="level"></param>
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








}
