using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Exceptions
{
    public class MissingEndCharException : Exception
    {
        public MissingEndCharException(char ch, char need, int line) : base($"第{line}行缺失与{ch}匹配的字符{need}\n")
        {
        }
    }
}
