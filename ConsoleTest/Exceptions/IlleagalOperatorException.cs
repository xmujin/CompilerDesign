using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Exceptions
{
    public class IlleagalOperatorException : Exception
    {
        

        public IlleagalOperatorException(string? errorToken, int line) : base($"第{line}行存在非法的运算符: {errorToken}\n")
        {
        }

        
        
    }
}
