using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace myapp.Command
{

    internal class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action<object?> _Excude {  get; set; }

        private Predicate<object?> _CanExcude { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="excude">执行的命令</param>
        /// <param name="canExcude">命令是否会执行的判断</param>
        public RelayCommand(Action<object?> excude, Predicate<object?> canExcude)
        {
            _Excude = excude;
            _CanExcude = canExcude;
        }

        public bool CanExecute(object? parameter)
        {
            return _CanExcude(parameter);
        }

        public void Execute(object? parameter)
        {
            _Excude(parameter);
        }
    }
}
