using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeNumsMining
{
    class PrimeNumsMiner
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter left and right boarders of a search range: ");
            int l = Convert.ToInt32(Console.ReadLine());
            int r = Convert.ToInt32(Console.ReadLine());
            /*
            Stopwatch sw = Stopwatch.StartNew();
            ThreadMiner(l, r);
            sw.Stop();
            Console.WriteLine("the prime numbers mining is done. Time: {0}", sw.Elapsed);
            Console.ReadKey();*/

            Stopwatch sw = Stopwatch.StartNew();
            List<int> list = TaskMiner(l, r);
            sw.Stop();
            Console.WriteLine("the prime numbers mining is done. Time: {0}", sw.Elapsed);


            PrintList(list);
            Console.ReadKey();
        }

        static bool IsPrime(int num)
        {
            if (num % 2 == 0 && num != 2 || num == 1) { return false; }
            for (int i = 3; i <= Math.Round(Math.Sqrt(num)); i += 2)
            {
                if (num % i == 0)
                    return false;
            }
            return true;
        }

        static List<int> ThreadMiner(int l, int r)
        {
            bool[] are_nums_prime = new bool[r + 1];
            for (int j = 0; j < are_nums_prime.Length; j++)
                are_nums_prime[j] = false;

            int thread_count = 7;
            List<Thread> threads = new List<Thread>();

            int step = (r - l) / thread_count;

            int i = l;
            for (; i < r - (2 * step - 1); i += step)
            {
                int left = i;
                int right = i + step - 1;
                Thread thread = new Thread(() => CheckPrimeNumsInRange(ref are_nums_prime, left, right));
                threads.Add(thread);
                thread.Start();
            }
            Thread lastthread = new Thread(() => CheckPrimeNumsInRange(ref are_nums_prime, i, r));
            threads.Add(lastthread);
            lastthread.Start();

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            return MinePrimeNums(are_nums_prime);
        }

        static List<int> TaskMiner(int l, int r)
        {
            bool[] are_nums_prime = new bool[r + 1];
            for (int j = 0; j < are_nums_prime.Length; j++)
                are_nums_prime[j] = false;

            int task_count = 7;
            List<Task> tasks = new List<Task>();

            int step = (r - l) / task_count;

            int i = l;
            for (; i < r - (2 * step - 1); i += step)
            {
                int left = i;
                int right = i + step - 1;
                Task task = new Task(() => CheckPrimeNumsInRange(ref are_nums_prime, left, right));
                tasks.Add(task);
                task.Start();
            }
            Task lasttask = new Task(() => CheckPrimeNumsInRange(ref are_nums_prime, i, r));
            tasks.Add(lasttask);
            lasttask.Start();

            foreach (Task ftask in tasks)
            {
                ftask.Wait();
            }

            return MinePrimeNums(are_nums_prime);
        }

        static ThreadPoolMiner(int l, int r)
        {

        }

        static List<int> MinePrimeNums(bool[] are_nums_prime)
        {
            List<int> primeNums = new List<int>();
            for (int i = 0; i < are_nums_prime.Length; i++)
                if (are_nums_prime[i])
                    primeNums.Add(i);
            return primeNums;
        }



        static void CheckPrimeNumsInRange(ref bool[] are_nums_prime, int l, int r)
        {
            for (int i = l; i <= r; i++)
            {
                are_nums_prime[i] = IsPrime(i);
            }

        }

        static void PrintList(List<int> l)
        {
            foreach (int i in l)
            {
                Console.Write("{0} ", i);
            }
        }
    }
}
