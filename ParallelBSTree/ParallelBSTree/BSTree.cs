using System;

namespace ParallelBSTree
{
    class RBTree// <TKey, TValue>
    {
        private Node root = null;
            
        internal Node InsertNode(int key, int val)
        {
            if (root == null)
                return root = new Node(key, val);
            
            Node current = root;
            Node parent = null;

            while (current != null)
            {
                if (current.key.CompareTo(key) == 0)
                    return current;
                parent = current;
                current = key.CompareTo(current.key) < 0 ? 
                    current.left : current.right;
            }

            //setup new node
            Node x = new Node(key, val, parent);
           
            //insert node in tree
            if (parent != null)
            {
                if (key.CompareTo(parent.data) < 0)
                    parent.left = x;
                else
                    parent.right = x;
            }
            else
                root = x;

            return x;
        }

       
        internal void DeleteNode(Node z)
        {
            Node y, x = null;

            if (z == null) return;

            if (z.left == null || z.right == null)
            {
                y = z;
            }
            else
            {
                y = z.right;
                while (y.left != null)
                    y = y.left;
            }

            //x is y`s only child
            
            x = y.left ?? y.right;

            //remove y from the parent chain
           
            if (x != null) x.parent = y.parent;
            if (y.parent != null)
            {
                if (y == y.parent.left)
                    y.parent.left = x;
                else
                    y.parent.right = x;
            }
            else
            {
                root = x;
            }

            if (y == z) return;
            
            z.key = y.key;
            z.data = y.data;
        }

        internal Node FindNode(int key)
        {
            var current = root;
            while (current != null)
            {
                if (key.CompareTo(current.key) == 0)
                {
                    return (current);
                }
                else
                    current = key.CompareTo(current.key) < 0 ?
                        current.left : current.right;
            }
            return null;
        }

        internal Node GetRoot()
        {
            return root;
        }

        internal bool IsEmpty()
        {
            return root == null;
        }

        internal void PrintTree(Node x)
        {
            while (true)
            {
                if (this.IsEmpty())
                {
                    Console.WriteLine("Tree is empty");
                    return;
                }
                
                if (x.right != null) PrintTree(x.right);

                for (var i = 0; i < GetNodeHeight(x) * 2; i++)
                    Console.Write(" ");
                
                if (x.parent != null)
                {
                    Console.Write(x == x.parent.right ? "/" : "\\");
                }

                if (x.left != null)
                {
                    x = x.left;
                    continue;
                }
                break;
            }
        }

        private int GetNodeHeight(Node x)
        {
            int height = 0;
            Node current = root;
            while (current != null && x != current)
            {
                height++;
                current = x.key < current.key ? current.left : current.right;
            }

            if (current == null)
                return -1;
            return height;
        }
    }
}