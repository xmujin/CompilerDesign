using myapp.Model.Lexer;
using myapp.Model.Parser;
using myapp.ViewModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Wpf.Ui.Controls;
using Image = System.Windows.Controls.Image;
using myapp.Model.Inter;
using myapp.Model.CodeGen;
using System.Windows.Documents;
using System.Text;
using Newtonsoft.Json;
using CefSharp.DevTools.Autofill;
using myapp.Model.Symbols;
using myapp.Model.TargetCode;

namespace myapp.View
{
    enum UseOperation 
    {

        LexicalAnalysis,
        Parser,
        SemanticAnalysis,
        IntermediateCode


    }



    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel mainViewModel;
        public MainWindow()
        {
            InitializeComponent();

            // 在这里实例化ViewModel, 并使用上下文
            mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;
        }


        private void ToOperation(UseOperation operation)
        {
            if (operation == UseOperation.LexicalAnalysis)
            {
                lexerCodeBar.Visibility = Visibility.Visible;
                lexerCode.Visibility = Visibility.Visible;
                parserCode.Visibility = Visibility.Hidden;
                genCode.Visibility = Visibility.Hidden;
            }
            else if(operation == UseOperation.Parser) // 语法分析
            {
                // 隐藏词法分析文本框
                lexerCodeBar.Visibility = Visibility.Hidden;
                // 隐藏中间代码生成
                genCode.Visibility= Visibility.Hidden;
                parserCode.Visibility= Visibility.Visible;
            }
            else if(operation == UseOperation.SemanticAnalysis) // 语义
            {
                lexerCodeBar.Visibility = Visibility.Visible;
                parserCode.Visibility = Visibility.Hidden;
                genCode.Visibility = Visibility.Hidden;
            }
            else if( operation == UseOperation.IntermediateCode) 
            {
                lexerCodeBar.Visibility = Visibility.Hidden;
                parserCode.Visibility = Visibility.Hidden;
                genCode.Visibility = Visibility.Visible;

            }
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(mainViewModel.SourceCode);
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "文本文件 |*.txt"; // Filter files by extension
                                           // Show open file dialog box
            bool? result = dialog.ShowDialog();
            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                string content = File.ReadAllText(filename);
                code.Text = content;
            }
        }


            /// <summary>
            /// 词法分析
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Lexer_Click(object sender, RoutedEventArgs e)
            {

                
                ToOperation(UseOperation.LexicalAnalysis);

                Lexer lex = new Lexer(code.Text);
                lexerCode.Text = lex.GetTokens();


            }

        /// <summary>
        /// 语法分析   使用浏览器显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Parser_Click(object sender, RoutedEventArgs e)
        {
            Program.symbolTableManager = new SymbolTableManager();
            Lexer lex = new Lexer(code.Text);
            Parser parser = new Parser(lex);
            StringBuilder sb = new StringBuilder();
            parser.Program();
            sb.Append("<!DOCTYPE html>\r\n<html lang=\"zh\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>JSON Editor</title>\r\n    <link rel=\"stylesheet\" href=\"./jsoneditor.min.css\">\r\n    <style>\r\n        body, html {\r\n            height: 100%;\r\n            margin: 0;\r\n            font-family: Arial, sans-serif;\r\n        }\r\n        #jsoneditor {\r\n            width: 100%;\r\n            height: 100%;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div id=\"jsoneditor\"></div>\r\n    <script src=\"./jsoneditor.min.js\"></script>\r\n    <script>\r\n        const container = document.getElementById('jsoneditor');\r\n        const options = {};\r\n        const editor = new JSONEditor(container, options);\r\n        const json =");
            string json = JsonConvert.SerializeObject(parser.js);
            sb.Append(json);
            sb.Append("\r\n;\r\n        editor.set(json);\r\n    </script>\r\n</body>\r\n</html>\r\n");
            File.WriteAllText(@"D:\work\CompilerDesign\myapp\Web\index.html", sb.ToString());
            
            
            ToOperation(UseOperation.Parser);
            parserCode.Address  = @"D:\work\CompilerDesign\myapp\Web\index.html";


        }

        /// <summary>
        /// 语义分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Sem_Click(object sender, RoutedEventArgs e)
        {
            Program.symbolTableManager = new SymbolTableManager();
            //Console.WriteLine("sdfsdf");
            //right.Visibility = Visibility.Hidden;
            ToOperation(UseOperation.SemanticAnalysis);
            Lexer lex = new Lexer(code.Text);
            //Console.WriteLine(lex.GetTokens());
            Parser parser = new Parser(lex);
            parser.Program();
            lexerCode.Text = Program.symbolTableManager.PrintSymbolTable();
            //lexerCode.Text = Node.symbolTable.PrintScopeHistory();




        }


        /// <summary>
        /// 生成中间代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Code_Click(object sender, RoutedEventArgs e)
        {

            Program.symbolTableManager = new SymbolTableManager();
            ToOperation(UseOperation.IntermediateCode);


            Lexer lex = new Lexer(code.Text);
            //Console.WriteLine(lex.GetTokens());
            Parser parser = new Parser(lex);
            parser.Program();
            //res.Text = CodeGen.ShowQuadruple(parser.quadruples);
            genCode.ItemsSource = parser.quadruples;


        }


        /// <summary>
        /// 目标代码生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Target_Click(object sender, RoutedEventArgs e)
        {
            Program.symbolTableManager = new SymbolTableManager();
            ToOperation(UseOperation.SemanticAnalysis);
            Lexer lex = new Lexer(code.Text);
            Parser parser = new Parser(lex);
            parser.Program();
            GenTarget genTarget = new GenTarget(parser.quadruples);

            lexerCode.Text = genTarget.GenAll(Program.symbolTableManager);
        }



    }
    
}
