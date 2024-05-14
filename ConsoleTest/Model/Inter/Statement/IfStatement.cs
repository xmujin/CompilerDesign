using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace myapp.Model.Inter
{
    /// <summary>
    /// If语句
    /// </summary>
    public class IfStatement : Statement
    {
        /// <summary>
        /// if语句的表达式
        /// </summary>
        [JsonProperty(Order = 2)]
        public Expression test;



        /// <summary>
        /// 表示if语句后的语句，可以是block语句，也可以是单个语句
        /// ，或是表达式语句
        /// </summary>
        [JsonProperty(Order = 3)]
        public Node consequent;


        public Node alternate;

        public IfStatement(Expression test, Node consequent) : base("IfStatement")
        {
            this.test = test;
            this.consequent = consequent;
        }
    }
}
