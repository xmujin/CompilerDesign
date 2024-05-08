using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class Node
    {
        int lexline = 0;
        public Node()
        {
            
        }

        public void Error(string s)
        {

        }

        static int lables;

        public int Newlabel()
        {
            return lables++;
        }



        /// <summary>
        /// 输出标签
        /// </summary>
        /// <param name="i"></param>
        public void Emitlabel(int i)
        {
            Console.WriteLine("L" + i + ":");

        }


        public void Emit(string s) 
        {
            Console.WriteLine("\t" + s);
        }



    }
}
