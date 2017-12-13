using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;

namespace ParallelBSTree
{
    internal class Program
    {
        static void Main()
        {
            int min = 0;
            int max = 1000000;
            int[] insertKeys = new int[max];
            int[] deleteKeys = new int[max];
            int[] searchKeys = new int[max];
            
            Random randNum = new Random();
            
            for (var i = 0; i < insertKeys.Length; i++)
            {
                insertKeys[i] = randNum.Next(min, max);
            }
            
            for (var i = 0; i < deleteKeys.Length; i++)
            {
                int j = randNum.Next(min, max);
                deleteKeys[i] = insertKeys[j];
            }
            
            for (var i = 0; i < searchKeys.Length; i++)
            {
                int j = randNum.Next(min, max);
                searchKeys[i] = insertKeys[j];
            }
            
            

            var seqTree = new BsTree();;
            var parTree = new BsTree();

            //смешанная вставка и удаление
//            int[] arr = new int[]{0, 1, 2, 3, 4, 5, 6, 7, 8};
//            var tasks = new List<Task>();
//            for (int i = 0; i < arr.Length; i++)
//            {
//                var i1 = i;
//                tasks.Add(Task.Run(() => parTree.Insert(i1, i1)));
//                if (i > 2)
//                {
//                   tasks.Add(Task.Run(() => parTree.Remove(i1 - 3)));
//                }
//                
//            }
//            
//            Task.WaitAll(tasks.ToArray());
//            parTree.PrintTree(parTree.Root);
            
            //производительность
            Stopwatch time1 = Stopwatch.StartNew();
           
            foreach (int key in insertKeys)
            {
                seqTree.Insert(key, key);
            }
            
            time1.Stop();

            Console.WriteLine("Sequential insert: {0}", time1.Elapsed);
            
            Stopwatch time2 = Stopwatch.StartNew();
            
            foreach (int key in deleteKeys)
            {
                seqTree.Remove(key);
            }

            time2.Stop();

            Console.WriteLine("Sequential delete: {0}", time2.Elapsed);

            Stopwatch time4 = Stopwatch.StartNew();

            Parallel.ForEach (insertKeys, key => {    
                parTree.Insert(key, key);
            });
            
            Stopwatch time3 = Stopwatch.StartNew();
            
            foreach (int key in searchKeys)
            {
                seqTree.Search(key);
            }

            time3.Stop();

            Console.WriteLine("Sequential search: {0}", time3.Elapsed);
            
            time4.Stop();

            Console.WriteLine("Concurrent insert: {0}", time4.Elapsed);

            Stopwatch time5 = Stopwatch.StartNew();
            
            Parallel.ForEach (deleteKeys, key => {
                parTree.Remove(key);
            });
            
            time5.Stop();

            Console.WriteLine("Concurrent delete: {0}", time5.Elapsed);
            
            Stopwatch time6 = Stopwatch.StartNew();
            
            Parallel.ForEach (searchKeys, key => {
                parTree.Search(key);
            });
            
            time6.Stop();

            Console.WriteLine("Concurrent search: {0}", time6.Elapsed);
            
        }
    }
}