using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    /// <summary>
    /// If语句
    /// </summary>
    public class IfStatement : Statement
    {
        /// <summary>
        /// 表示if语句后的语句，可以是block语句，也可以是单个语句
        /// ，或是表达式语句
        /// </summary>
        public Statement consequent;
        public IfStatement(Statement consequent) : base("IfStatement")
        {
            this.consequent = consequent;
        }
    }
}
