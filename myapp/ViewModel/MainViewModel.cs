#define AUTO
using myapp.Command;
using myapp.Model.Lexer;
using System.IO;
using System.Security.RightsManagement;
using System.Text;
using System.Windows.Documents;
using System.Windows.Input;

namespace myapp.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private string? _SourceCode;

        public string? SourceCode
        {
            get { return _SourceCode; }
            //set { _SourceCode = value; OnPropertyChanged(nameof(SourceCode)); }
            set
            {
                _SourceCode = value;
                //LexicalAnalyzer(null);
                OnPropertyChanged(nameof(SourceCode)); // 通知属性更改
            }
        }
                

    


        private string _LexerCode;

        public string LexerCode
        {
            get { return _LexerCode; }
            set { _LexerCode = value; OnPropertyChanged(nameof(LexerCode)); }
        }


        private string _OutMessage;
        public string OutMessage
        {
            get { return _OutMessage; }
            set { _OutMessage = value; OnPropertyChanged(nameof(OutMessage)); }
        }

        private FlowDocument _richTextContent;

        public FlowDocument RichTextContent
        {
            get { return _richTextContent; }
            set
            {

                    _richTextContent = value;
                    OnPropertyChanged(nameof(RichTextContent));
                
            }
        }



        public ICommand ShowFileDialogCommand { get; set; }
        public ICommand LexicalAnalyzerCommand { get; set; }
        public ICommand ParserCommand { get; set; }


        public MainViewModel()
		{
            ShowFileDialogCommand   = new RelayCommand(ShowFileDialog, CanShowFileDialog);
            LexicalAnalyzerCommand  = new RelayCommand(LexicalAnalyzer, CanLexicalAnalyzer);
            ParserCommand = new RelayCommand(Parser, CanParser);
        }


		public bool CanShowFileDialog(object? obj)
		{
			return true;
		}

		public void ShowFileDialog(object? obj)
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
                SourceCode = content;
            }
        }

        public bool CanLexicalAnalyzer(object? obj)
        {
            return true;
        }

        public bool CanParser(object? obj)
        {
            return true;
        }


        /// <summary>
        /// 词法分析
        /// </summary>
        /// <param name="obj"></param>
        public void LexicalAnalyzer(object? obj)
        {
#if AUTO
            Lexer lexer = new Lexer(SourceCode);
            LexerCode = lexer.GetTokens();


            OutMessage = lexer.GetErrors();

#else
            // 
            Lexer lexer = new Lexer(SourceCode);
            StringBuilder sb = new StringBuilder();

            List<Token?>? tokens = lexer.NewScan();
            foreach(Token? token in tokens)
            {
                sb.Append(token);
            }
            LexerCode = sb.ToString();
            OutMessage = lexer.ErrorString.ToString();
#endif
        }

        public void Parser(object? obj)
        {

            Lexer lexer = new Lexer(SourceCode);
            LexerCode = lexer.GetTokens();

        }

    }
}
