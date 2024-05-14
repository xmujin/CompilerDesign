using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class BlockStatement : Statement
    {


        /// <summary>
        /// 语句块是一系列语句或声明构成
        /// </summary>
        public List<Node> body = new List<Node>();
        public BlockStatement() : base("BlockStatement")
        {

        }

    }
}
