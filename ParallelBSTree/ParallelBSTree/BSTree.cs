using System;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace ParallelBSTree
{
    class BsTree // <TKey, TValue>
    {
        public Node Root = null;

        internal Node Insert(int key, int val)
        {
            lock (this)
            {
                if (Root == null)
                    return Root = new Node(key, val);
            }
            
            Node current = Root;

            while (current != null)
            {
                if (current.parent != null)
                    lock (current.parent)
                    {
                        lock (current)
                        {
                            if (current.key.CompareTo(key) == 0)
                                return current;
                            
                            if (key < current.key)
                            {
                                if (current.left == null)
                                    return current.left = new Node(key, val, current);
                                
                                lock (current.left)
                                    current = current.left;
                                continue;
                            }
                            
                            if (current.right == null)
                                return current.right = new Node(key, val, current);
                            
                            
                            lock (current.right)
                                current = current.right;
                        }
                    }
                else
                    lock (current)
                    {
                        if (current.key.CompareTo(key) == 0)
                            return current;
                            
                        if (key < current.key)
                        {
                            if (current.left == null)
                                return current.left = new Node(key, val, current);
                                
                            lock (current.left)
                                current = current.left;
                            continue;
                        }
                            
                        if (current.right == null)
                            return current.right = new Node(key, val, current);
                            
                            
                        lock (current.right)
                            current = current.right;
                    }
                
                
            }
            return null;
        }


        internal void Remove(int key)
        {
            while (true)
            {
                var z = Search(key);
                if (z == null) return;
    
                var s = GetSuccessor(z);
    
                if (z.parent != null)
    
                    try
                    {
                        lock (z.parent)
                        {
                            lock (z)
                            {
                                try
                                {


                                    
                                    if (s != null)
                                        lock (s)
                                        {
                                            if (z.left == null || z.right == null)
                                            {
                                                if (z.parent.left == z)
                                                    z.parent.left = s;
                                                else
                                                    z.parent.right = s;
                                                s.parent = z.parent;
                                            }
                                            else
                                            {
                                                if (z.right == s)
                                                {
                                                    if (z.parent.left == z)
                                                        z.parent.left = s;
                                                    else
                                                        z.parent.right = s;
                                                    s.parent = z.parent;
                                                    s.left = z.left;
                                                }
                                                else
                                                {
                                                    z.key = s.key;
                                                    z.data = s.data;
                                                    s.parent.left = s.right;
                                                    if (s.right != null)
                                                        s.right.parent = s.parent;
                                                }
                                            }
                                        }
                                    else if (z.parent.left == z)
                                        z.parent.left = null;
                                    else
                                        z.parent.right = null;
                                }
                                catch (Exception ex)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                else
                    lock (z)
                    {
                        if (s != null)
                            try
                            {
                                lock (s)
                                {
                                    if (z.left == null || z.right == null)
                                    {
                                        s.parent = null;
                                        Root = s;
                                    }
                                    else
                                    {
                                        if (z.right == s)
                                        {
                                            s.parent = null;
                                            s.left = z.left;
                                            z.left.parent = s;
                                            Root = s;
                                        }
                                        else
                                        {
                                            s.parent.left = s.right;
                                            if (s.right != null)
                                                s.right.parent = s.parent;
                                            s.parent = null;
                                            z.left.parent = s;
                                            Root = s;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        
                        else
                            Root = null;
                    }
        }
    }

        private Node GetSuccessor(Node current)
        {
            switch (current.left)
            {
                case null when current.right == null:
                    return null;
                case null:
                    return current.right;
                default:
                    if (current.right == null)
                        return current.left;

                    return Min(current.right);
            }
        }

        private Node Min(Node z)
        {
            while (true)
            {
                lock (z)
                {
                    if (z.left == null)
                        return z;
                }
                z = z.left;
            }
        }

        internal Node Search(int key)
        {
            var current = Root;
            if (null == current)
                return null;
            
            while (current != null)
            {
                if (current.parent != null)
                    lock (current.parent)
                    {
                        lock (current)
                        {
                            if (key.CompareTo(current.key) == 0)  
                                return current;
                            current = key.CompareTo(current.key) < 0 ? current.left : current.right;
                        }
                    }
                else
                    lock (current)
                    {
                        if (key.CompareTo(current.key) == 0)  
                            return current;
                        current = key.CompareTo(current.key) < 0 ? current.left : current.right;
                    }
            }
            return null;
        }
        
        internal void PrintTree (Node x)
        {
            if (Root == null)
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
            Node current = Root;
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