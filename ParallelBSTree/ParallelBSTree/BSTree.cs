using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ParallelBSTree
{
    class BsTree // <TKey, TValue>
    {
        public Node _root = null;

        internal Node Insert(int key, int val)
        {
            while (true)
            {
                if (_root == null)
                    return _root = new Node(key, val);

                Node current = _root;
                Node parent = null;

                while (current != null)
                {
                    if (current.key.CompareTo(key) == 0)
                        return current;
                    parent = current;
                    current = key.CompareTo(current.key) < 0 ? current.left : current.right;
                }
                try
                {
                    lock (parent)
                    {
                        Node x = new Node(key, val, parent);

                        //insert node in tree
                        if (key.CompareTo(parent.data) < 0)
                            parent.left = x;
                        else
                            parent.right = x;

                        return x;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }


        internal void Delete(Node z)
        {
            while (true)
            {

                if (z == null) return;

                var victim = FindVictim(z);
                var orphan = victim.right;

                try
                {
                    lock (z)
                    {
                        lock (victim)
                        {
                            if (FindVictim(z) != victim || orphan != null && orphan.parent != victim)
                                continue;
                              
                            if (orphan != null) orphan.parent = victim.parent;
                            if (victim.parent != null)
                            {
                                if (victim == victim.parent.left)
                                    victim.parent.left = orphan;
                                else
                                    victim.parent.right = orphan;
                            }
                            else
                            {
                                _root = orphan;
                            }

                            if (victim == z) return;

                            z.key = victim.key;
                            z.data = victim.data;
                        }

                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }

        }

        private Node FindVictim(Node z)
        {
            Node victim = null;
            if (z.left == null || z.right == null)
            {
                victim = z;
            }
            else
            {
                victim = z.right;
                while (victim.left != null)
                    victim = victim.left;
            }
            return victim;
        }

        internal Node FindNode(int key)
        {
            while (true) {
                var current = _root;
                while (current != null)
                {
                    if (key.CompareTo(current.key) == 0)
                    {
                        try
                        {
                            lock (current)
                            {
                                return (current);
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                        
                    }
                    current = key.CompareTo(current.key) < 0 ? current.left : current.right;
                }
                return null;
            }
        }
        
        internal void PrintTree (Node x)
        {
            if (_root == null)
            {
                Console.WriteLine("Tree is empty");
                return;
            }
            if (x.right != null) PrintTree(x.right);

            for (int i = 0; i < GetNodeHeight(x) * 2; i++)
                Console.Write(" ");
            if (x.parent != null)
            {
                if (x == x.parent.right)
                    Console.Write("/");
                else Console.Write("\\");
            }
            
                Console.WriteLine(x.data);
            

            if (x.left != null) PrintTree(x.left);           
        }
        
        private int GetNodeHeight(Node x)
        {
            int height = 0;
            Node current = _root;
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