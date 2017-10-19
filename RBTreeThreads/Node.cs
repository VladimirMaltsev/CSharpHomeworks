using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw13October
{
    class Node
    {
        public Node left;
        public Node right;
        public Node parent;

        public eColor color = eColor.RED;

        public int data;
        public int key;
        
        public Node(int key, int val)
        {
            this.key = key;
            this.data = val;
            this.left = null;
            this.right = null;
            this.parent = null;
        }
        //конструктор, в котором можно указать родителя
        public Node(int key, int data, Node parent) : this(key, data)
        {
            this.parent = parent;
        }
    }
}
