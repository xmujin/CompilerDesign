using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class Program : Node
    {

        [JsonProperty(Order = 2)]
        public List<Node> body = new List<Node>();
        public Program() : base("Program")
        {
        }

        
    }
}
