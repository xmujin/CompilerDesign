using myapp.Model.CodeGen;
using myapp.Model.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace myapp.Model.Inter
{
    public class Node
    {
        public static SymbolTableManager symbolTableManager = new SymbolTableManager();
        public static int depth = 0; // 清零

        /// <summary>
        /// 抽象节点的类型
        /// </summary>
        [JsonProperty(Order = 1)]
        public string type { get; set; } = "Node";

        public Node(string type)
        {
            this.type = type;
        }
        public Node()
        {
        }

        public static int labels = 0;

        public int NewLabel()
        {
            return ++labels;
        }

        public void EmitLabel(List<Quadruple> quadruples,int i)
        {
            quadruples.Add(new Quadruple("label", "L" + i, null, null));
        }

        public void EmitLabel(List<Quadruple> quadruples, string name)
        {
            quadruples.Add(new Quadruple("label", name, null, null));
        }



        public virtual void Gen(List<Quadruple> quadruples, int b, int a)
        {
            
        }

        /*public virtual List<Quadruple> Gen(List<Quadruple> quadruples, ref int line)
        {
            return null;
        }*/

    }
}
