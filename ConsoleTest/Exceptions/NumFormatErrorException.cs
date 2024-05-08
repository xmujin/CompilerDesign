using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Exceptions
{
    public class NumFormatErrorException : Exception
    {
        public NumFormatErrorException(string? message, int line) : base($"第{line}行出现数字格式错误: {message}\n")
        {
        }
    }
}
