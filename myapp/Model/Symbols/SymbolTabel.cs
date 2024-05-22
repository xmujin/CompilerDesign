using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace myapp.Model.Symbols
{

    public class SymbolTable
    {
        public readonly Dictionary<string, Symbol> _symbols;
        public SymbolTable Parent { get; }
        public readonly List<SymbolTable> _children;

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
        /// 打印变量符号表
        /// </summary>
        /// <param name="level"></param>
        public string PrintVarSymbols(int level = 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var symbol in _symbols.Values)
            {

                if (symbol is VariableSymbol vs)
                {
                    vs.scope = level + "";
                    stringBuilder.AppendLine(string.Format("|{0,-10}|{1,-9}|{2,-9}|{3,-10}|{4,-10}|", vs.name, vs.size, vs.type, "0", vs.scope));
                    stringBuilder.AppendLine("--------------------------------------------------------");
                }
            }
            foreach (var child in _children)
            {
                stringBuilder.Append(child.PrintVarSymbols(level + 1));

            }
            return stringBuilder.ToString();
        }

        public string PrintFunSymbols()
        {

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("------------------------");
            stringBuilder.AppendLine(string.Format("|{0,-19}|", "函数表"));
            stringBuilder.AppendLine("------------------------");
            stringBuilder.AppendLine(string.Format("|{0,-7}|{1,-7}|", "函数名", "返回类型"));
            stringBuilder.AppendLine("------------------------");
            foreach (var symbol in _symbols.Values)
            {

                if (symbol is FunctionSymbol)
                {
                    stringBuilder.AppendLine(string.Format("|{0,-10}|{1,-11}|", symbol.name, symbol.type));
                }
            }
            stringBuilder.AppendLine("------------------------");
            return stringBuilder.ToString();
        }



    }








}
