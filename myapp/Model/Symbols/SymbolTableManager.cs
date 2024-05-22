using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace myapp.Model.Symbols
{

    public class SymbolTableManager
    {
        public SymbolTable _currentScope;
        public readonly SymbolTable _globalScope;

        public SymbolTableManager()
        {
            _globalScope = new SymbolTable();
            _currentScope = _globalScope;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <exception cref="Exception"></exception>
        public void DefineVariable(string name, int size, string type, string init, string scope)
        {
            var variable = new VariableSymbol(name, size, type, init, scope);
            if (!_currentScope.AddSymbol(variable))
            {
                throw new Exception($"Variable {name} already defined in the current scope.");
            }
        }


        public void DefineFunction(string name, string type)
        {
            var function = new FunctionSymbol(name, type);
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

        public string PrintSymbolTable()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("------------------------------------------------------");
            stringBuilder.AppendLine(string.Format("|{0,-47}|", "变量符号表"));
            stringBuilder.AppendLine("------------------------------------------------------");
            stringBuilder.AppendLine(string.Format("|{0,-7}|{1,-7}|{2,-7}|{3,-7}|{4,-7}|", "变量名", "长度", "类型", "初始值", "作用域"));
            stringBuilder.AppendLine("--------------------------------------------------------");
            stringBuilder.Append(_globalScope.PrintVarSymbols());
            stringBuilder.Append(_globalScope.PrintFunSymbols());


            return stringBuilder.ToString();    
        }
    }
}
