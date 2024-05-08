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

namespace myapp.View
{
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(mainViewModel.SourceCode);
        }

        public void Parser_Click(object sender, RoutedEventArgs e) 
        {

            string fileContent = File.ReadAllText(@"H:\MyTests\compile\myapp\myapp\test.txt");
            //Lexer lex = new Lexer("{\r\nint j; \r\nj = 3 + 2;\r\n}");
            Lexer lex = new Lexer(fileContent);
            Console.WriteLine(lex.GetTokens());

            Parser parser = new Parser(lex);
            parser.Program();
            //Console.WriteLine("Hello, World!");
            // 定义 DOT 文件路径和图像文件路径




            string dotFilePath = @"H:\MyTests\compile\myapp\myapp\test.dot";
            string pngFilePath = @"H:\MyTests\compile\myapp\myapp\binary_tree.png";
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

            // 创建BitmapImage对象
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(@"H:\MyTests\compile\myapp\myapp\binary_tree.png", UriKind.Absolute);
            bitmap.EndInit();


            img.Source = bitmap;
            img.Stretch = Stretch.Uniform;
            // 将BitmapImage对象设置为Image控件的Source

            viewer.Visibility = Visibility.Visible;













        }

        private void MenuItem_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }
    }
}