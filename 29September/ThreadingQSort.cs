using System;
using System.Threading;

namespace Homework29September
{
    class QSorter
    {
        static int[] nums;
        public static void Main(string[] args)
        {
            InitArray(out nums, 20000000);
            //PrintArray(nums);

            var before = DateTime.UtcNow;
            Qsort_thread(0, nums.Length - 1);
            var spendTime = DateTime.UtcNow - before;
            
            Console.WriteLine("Time : {0}", spendTime);
            //PrintArray (nums);

            Console.Read();
        }

        private static void Qsort_thread(int left, int right)
        {
            if (left < right)
            {
                int mid = Partion(left, right);

                Properties p = new Properties
                {
                    left = mid,
                    right = right
                };

                Thread t = new Thread(new ParameterizedThreadStart(Run));
                t.Start(p);

                Qsort(left, mid - 1);
            }
        }

        private static void Qsort(int left, int right)
        {
            if (left < right)
            {
                int mid = Partion(left, right);
                Qsort(left, mid - 1);
                Qsort(mid, right);
            }
        }

        private static void Run(Object o)
        {
            Properties p = (Properties)o;
            Qsort(p.left, p.right);
        }

        static int Partion(int l, int r)
        {
            int mid_val = nums[l + (r - l) / 2];

            while (l <= r)
            {

                while (nums[l] < mid_val)
                    ++l;
                while (nums[r] > mid_val)
                    --r;

                if (l <= r)
                {
                    Swap(l, r);
                    l++;
                    r--;
                }

            }
            return l;
        }

        static void Swap(int i, int j)
        {
            /*nums[i] = nums[j] + nums[i];
            nums[j] = nums[i] - nums[j];
            nums[i] = nums[i] - nums[j];*/
            int tmp = nums[i];
            nums[i] = nums[j];
            nums[j] = tmp;
        }

        static void InitArray(out int[] arr, int n)
        {
            Random random = new Random();
            arr = new int[n];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = random.Next() % 10000;
            }
        }

        static void PrintArray(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i]);
                Console.Write(" ");
            }
        }
    }


    public class Properties
    {
        public int left;
        public int right;
    }
}
        

