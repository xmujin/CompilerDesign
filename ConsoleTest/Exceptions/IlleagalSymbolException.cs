
using System.Runtime.Serialization;

namespace myapp.Exceptions
{
    internal class IlleagalSymbolException : Exception
    {

        string? errorToken;
        int line;
        char that;

        
        public IlleagalSymbolException(string? errorToken, char that, int line) : base($"在第{line}行{errorToken}中检测到非法字符: {that}\n")
        {
            this.errorToken = errorToken;
            this.line = line;
            this.that = that;
        }

        
    }
}
