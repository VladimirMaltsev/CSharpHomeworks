using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;

namespace ParallelBSTree
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var r = new Random();
            int[] arr = new int[10];
            for (var i = 0; i < arr.Length; i ++) 
                arr[i] = r.Next() % 100;
           
           
            var tree = new BsTree();
            var dog = Stopwatch.StartNew();
            var tasks = new List<Task>();
            for (var i = 0; i < arr.Length; i++)
            {
                var i1 = i;
                var task = new Task(()  => tree.Insert(arr[i1], arr[i1]));
                tasks.Add(task);
                task.Start();
            }
            Task.WaitAll(tasks.ToArray());
            dog.Stop();
            Console.WriteLine("Parallel insert of {1} elems done. Time elapsed: {0}", dog.Elapsed, arr.Length);
            tree.PrintTree(tree._root);
            tree = new BsTree();
            dog.Restart();
            for (var i = 0; i < arr.Length; i++)
            {
                tree.Insert(arr[i], arr[i]);
            }
            dog.Stop();
            Console.WriteLine("Sequential insert of {1} elems done. Time elapsed: {0}", dog.Elapsed, arr.Length);
            tree.PrintTree(tree._root);
            Console.ReadKey();
        }
    }
}