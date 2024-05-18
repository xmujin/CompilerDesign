using myapp.Model.CodeGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace myapp.Model.Inter
{
    public class BlockStatement : Statement
    {


        /// <summary>
        /// 语句块是一系列语句或声明构成
        /// </summary>
        public List<Node> body;
        public BlockStatement() : base("BlockStatement")
        {

        }


        /// <summary>
        /// 生成块语句序列
        /// </summary>
        /// <param name="quadruples"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public override void Gen(List<Quadruple> quadruples, int b, int a)
        {
            // 处理变量声明
            foreach(var node in body) 
            {
                if(node is VariableDeclaration variableDeclaration)
                {

                    variableDeclaration.Gen(quadruples, b, a);

                }
                else if(node is IfStatement ifs)
                {
                    //int begin = NewLabel();
                    int end = NewLabel();
                    ifs.Gen(quadruples,0, end);
                }
                else // 其他语句
                {
                    
                    node.Gen(quadruples, b, a);

                }



            
            }


        }

    }
}
