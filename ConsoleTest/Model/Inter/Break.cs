using myapp.Model.Inter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = myapp.Model.Symbols.Type;
namespace ConsoleTest.Model.Inter
{
    public class Break : Stmt
    {
        Stmt stmt;

        public Break()
        {
            if (Stmt.Enclosing == Stmt.Null) Error("未包围的break");
            stmt = Stmt.Enclosing; // 
        }


        public override void Gen(int b, int a)
        {
            base.Gen(b, a);
        }
    }
}
