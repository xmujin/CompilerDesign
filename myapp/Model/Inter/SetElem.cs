using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{

    /// <summary>
    /// 实现对数组元素的赋值
    /// </summary>
    public class SetElem : Stmt
    {
        public Id array;
        public Expr index;
        public Expr expr;
        public SetElem(Access x, Expr y)
        {

        }

    }
}
