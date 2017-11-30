namespace ParallelBSTree
{
    public class Node
    {
        public Node left;
        public Node right;
        public Node parent;

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