using System;
using System.Collections.Generic;

public class SymbolTable
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
    public void AddSymbol(string name, SymbolType type)
    {
        if (scopeStack.Count > 0)
        {
            Scope currentScope = scopeStack.Peek();
            currentScope.AddSymbol(name, type);
        }
        else
        {
            throw new InvalidOperationException("No scope available.");
        }
    }

    // 查找符号
    public SymbolInfo LookupSymbol(string name)
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
            Console.WriteLine($"Current Scope: {currentScope.Name}");
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

        Console.WriteLine("Scope History:");
        foreach (var scope in scopeHistory)
        {
            //Console.WriteLine($"Scope: {scope.Name}");
            scope.PrintSymbols();
        }
    }
}

public class Scope
{
    public string Name { get; }
    private Dictionary<string, SymbolInfo> symbols;

    public Scope(string name)
    {
        Name = name;
        symbols = new Dictionary<string, SymbolInfo>();
    }

    public void AddSymbol(string name, SymbolType type)
    {
        if (!symbols.ContainsKey(name))
        {
            symbols[name] = new SymbolInfo(name, type);
        }
        else
        {
            throw new ArgumentException($"Symbol '{name}' already exists in scope '{Name}'.");
        }
    }

    public SymbolInfo LookupSymbol(string name)
    {
        return symbols.ContainsKey(name) ? symbols[name] : null;
    }

    public void PrintSymbols()
    {
        foreach (var symbol in symbols.Values)
        {
            
            Console.WriteLine($"  {symbol.Name} : {symbol.Type}");
        
        
        
        }
    }
}

public class SymbolInfo
{
    public string Name { get; }
    public SymbolType Type { get; }

    public SymbolInfo(string name, SymbolType type)
    {
        Name = name;
        Type = type;
    }
}

public enum SymbolType
{
    Integer,
    Float,
    String,
    // Add more types as needed
}

class Program
{
    static void Main(string[] args)
    {
        SymbolTable symbolTable = new SymbolTable();

        // 添加一些符号到全局作用域
        symbolTable.AddSymbol("x", SymbolType.Integer);
        symbolTable.AddSymbol("y", SymbolType.Float);

        // 进入一个函数作用域
        symbolTable.EnterScope("Function1");

        // 添加一些符号到函数作用域
        symbolTable.AddSymbol("a", SymbolType.Integer);
        symbolTable.AddSymbol("b", SymbolType.String);

        // 进入一个嵌套作用域
        symbolTable.EnterScope("Block1");

        // 添加一些符号到嵌套作用域
        symbolTable.AddSymbol("c", SymbolType.Float);

        // 打印当前作用域的符号表
        symbolTable.PrintCurrentScope();

        // 离开嵌套作用域
        symbolTable.ExitScope();

        // 打印当前作用域的符号表
        Console.WriteLine("After exiting Block1:");
        symbolTable.PrintCurrentScope();

        // 离开函数作用域
        symbolTable.ExitScope();

        // 打印当前作用域的符号表
        Console.WriteLine("After exiting Function1:");
        symbolTable.PrintCurrentScope();

        // 打印所有作用域的历史记录
        Console.WriteLine("Scope History:");
        symbolTable.PrintScopeHistory();
    }
}
