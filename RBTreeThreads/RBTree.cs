using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw13October
{
    class RBTree// <TKey, TValue>
    {
        private Node root = null;

        private void RotateLeft (Node node)
        {
            Node rn = node.right;

            //establish node.right link
            node.right = rn.left;
            if (rn.left != null)
                rn.left.parent = node;

            //establish rn.parent link
            if (rn != null)
                rn.parent = node.parent;
            if (node.parent != null)
            {
                if (node == node.parent.left)
                    node.parent.left = rn;
                else
                    node.parent.right = rn;
            }
            else
                root = rn;

            //link node and rn
            rn.left = node;
            if (node != null)
                node.parent = rn;
        }

        private void RotateRight(Node node)
        {
            Node ln = node.left;

            //establish node.left link
            node.left = ln.right;
            if (ln.right != null)
                ln.right.parent = node;

            //establish ln.parent
            if (ln != null)
                ln.parent = node.parent;
            if (node.parent != null)
            {
                if (node == node.parent.right)
                    node.parent.right = ln;
                else
                    node.parent.left = ln;
            }
            else
                root = ln;

            //link node and ln
            ln.right = node;
            if (node != null)
                node.parent = ln;    
            
        }
            
        private void InsertFixup(Node x)
        {
            while (x != root && x.parent.color == eColor.RED )
            {
                if (x.parent == x.parent.parent.left)
                {
                    Node y = x.parent.parent.right;
                    if (y != null && y.color == eColor.RED)
                    {
                        //uncle is RED
                        x.parent.color = eColor.BLACK;
                        if (y != null) y.color = eColor.BLACK;
                        x.parent.parent.color = eColor.RED;
                        x = x.parent.parent;
                    } else
                    {
                        //uncle is BLACK
                        if (x == x.parent.right)
                        {
                            //make x a left child
                            x = x.parent;
                            RotateLeft(x);
                        }

                        //recolor and rotate
                        x.parent.color = eColor.BLACK;
                        x.parent.parent.color = eColor.RED;
                        RotateRight(x.parent.parent);
                    }
                } else
                {
                    //mirror image of above code
                    Node y = x.parent.parent.left;
                    if (y != null && y.color == eColor.RED)
                    {
                        //uncle is RED
                        x.parent.color = eColor.BLACK;
                        if (y != null) y.color = eColor.BLACK;
                        x.parent.parent.color = eColor.RED;
                        x = x.parent.parent;
                    } else
                    {
                        //uncle is BLACK
                        if (x == x.parent.left)
                        {
                            x = x.parent;
                            RotateRight(x);
                        }
                        x.parent.color = eColor.BLACK;
                        x.parent.parent.color = eColor.RED;
                        RotateLeft(x.parent.parent); 
                    }
                }
            }

            root.color = eColor.BLACK;
        }

        //private void InsertNode(TKey key, TValue val)
        internal Node InsertNode(int key, int val)
        {
            if (root == null)
            {
                root = new Node(key, val)
                {
                    color = eColor.BLACK
                };
                return root;
            }
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
            Node x = new Node(key, val)
            {
                parent = parent,
                color = eColor.RED
            };

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

            InsertFixup(x);
            return x;
        }

        private void DeleteFixup(Node x, Node curr_child)
        {
            while (curr_child != root && x.color == eColor.BLACK)
            {
                if (curr_child == x.parent.left)
                {
                    Node w = x.parent.right;
                    if (w.color == eColor.RED)
                    {
                        w.color = eColor.BLACK;
                        x.parent.color = eColor.RED;
                        RotateLeft(x.parent);
                        w = x.parent.right;
                    }
                    if ((w.left == null || w.left.color == eColor.BLACK) &&
                        (w.right == null || w.right.color == eColor.BLACK))
                    {
                        w.color = eColor.RED;
                        x = x.parent;
                        curr_child = x;
                    } else
                    {
                        if (w.right.color == eColor.BLACK)
                        {
                            w.left.color = eColor.BLACK;
                            w.color = eColor.RED;
                            RotateRight(w);
                            w = x.parent.right;
                        }
                        w.color = x.parent.color;
                        x.parent.color = eColor.BLACK;
                        w.right.color = eColor.BLACK;
                        RotateLeft(x.parent);
                        x = root;
                        curr_child = root;
                    }
                } else
                {
                    Node w = x.parent.left;
                    if (w.color == eColor.RED)
                    {
                        w.color = eColor.BLACK;
                        x.parent.color = eColor.RED;
                        RotateRight(x.parent);
                        w = x.parent.left;
                    }
                    if ((w.left == null || w.left.color == eColor.BLACK) &&
                        (w.right == null || w.right.color == eColor.BLACK))
                    {
                        w.color = eColor.RED;
                        x = x.parent;
                        curr_child = x;
                    } else
                    {
                        if (w.left != null && w.left.color == eColor.BLACK)
                        {
                            w.right.color = eColor.BLACK;
                            w.color = eColor.RED;
                            RotateLeft(w);
                            w = x.parent.left;
                        }
                        w.color = x.parent.color;
                        x.parent.color = eColor.BLACK;
                        if ( w.left != null) w.left.color = eColor.BLACK;
                        RotateRight(x.parent);
                        x = root;
                        curr_child = root;
                    }
                }
            }

            x.color = eColor.BLACK;
            if (curr_child != null) curr_child.color = eColor.BLACK;
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
            if (y.left != null)
                x = y.left;
            else
                x = y.right;

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

            if (y != z)
            {
                z.key = y.key;
                z.data = y.data;
            }

            if (y.color == eColor.BLACK)
                DeleteFixup(y, x);
        }

        internal Node FindNode(int key)
        {
            Node current = root;
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

        internal void PrintTree (Node x)
        {
            if (this.IsEmpty())
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
            if (x.color == eColor.RED)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(x.data);
            } else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(x.data);
            }
            Console.ResetColor();

            if (x.left != null) PrintTree(x.left);           
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

