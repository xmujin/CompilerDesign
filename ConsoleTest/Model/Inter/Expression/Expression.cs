using myapp.Model.Lexer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class Expression : Node
    {
        [JsonProperty(Order = 2)]
        public Token op;

        [JsonProperty(Order = 3)]
        public Node left;

        [JsonProperty(Order = 4)]
        public Node right;

        public Expression(string type) : base(type)
        {
        }

        public Expression(Token op, Node left, Node right, string type) : base(type) 
        {
            this.op = op;
            this.left = left;
            this.right = right;
        }
    }
}
