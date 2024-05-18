using myapp.Model.Inter;
using myapp.Model.Lexer;
using myapp.Model.Parser;
using myapp.Model.Utils;
using Newtonsoft.Json;
using System.Diagnostics;



namespace myapp
{

    public class main
    {

        static void Main(string[] args)
        {


            string fileContent = File.ReadAllText(@"D:\work\CompilerDesign\ConsoleTest\test.txt");
            Lexer lex = new Lexer(fileContent);
           Console.WriteLine(lex.GetTokens());

            Parser parser = new Parser(lex);


            parser.Program();
            Program.symbolTable.PrintScopeHistory();
            string json = JsonConvert.SerializeObject(parser.js);
            File.WriteAllText(@"D:\work\CompilerDesign\ConsoleTest\sb.json", json);



            /*Dot a = new Dot(parser.GetTree());
            a.CreateDotFile();*/








#if false
            string dotFilePath = @"H:\work\CompilerDesign\ConsoleTest\test.dot";
            string pngFilePath = @"H:\work\CompilerDesign\ConsoleTest\binary_tree.png";
            string currentDirectory = Directory.GetCurrentDirectory();
            // 构建命令行命令
            string command = $"dot -Tpng {dotFilePath} -o {pngFilePath}";

            // 创建一个 ProcessStartInfo 对象，用于配置启动命令行的参数
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe"; // 指定要执行的命令行解释器
            startInfo.Arguments = $"/C {command}"; // 指定要执行的命令
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            // 启动一个 Process 对象，并使用指定的 ProcessStartInfo 参数启动命令行
            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();
            }
#endif


        }
    }
}
