using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw13October
{
    class Program
    {
        static void Main(string[] args)
        {
            RBTree tree = new RBTree();
            for (int i = 0; i < 100; i ++ )
            {
                tree.InsertNode(i, i);

            }
            tree.PrintTree(tree.GetRoot());
            for (int i = 0; i < 100; i++)
            {
                tree.DeleteNode(tree.FindNode(i));
                
            }
            tree.PrintTree(tree.GetRoot());
            Console.ReadKey();
        }
    }
}
