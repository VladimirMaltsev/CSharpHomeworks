using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    class ParallelHasher
    {
        static void Main(string[] args)
        {
            string path;
            while ((path = Console.ReadLine()) != "exit") { 
                if (File.Exists(path) || Directory.Exists(path))
                {
                    string hash = GetPathHash(path);
                    Console.WriteLine(hash);
                }
                else Console.WriteLine("directory does not exists");
            }
        }       

        static string GetPathHash (String path)
        {
            StringBuilder sBuilder = new StringBuilder();
            
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, path);
                sBuilder.Append(hash);
            }

            string[] elems;
            try {
                elems = Directory.EnumerateDirectories(path).ToArray();
            }
            catch (Exception ex) {
                elems = new string[0];
            }

            Task<string>[] tasks = new Task<string>[elems.Length]; 

            for (int i = 0; i < elems.Length; ++ i)
            {
                string elem = elems[i];
                tasks[i] = Task.Run(() => GetPathHash(elem));
            }

            Task.WaitAll(tasks);
            for (int i = 0; i < tasks.Length; i ++)
            {
                string elem_hash = tasks[i].Result;
                sBuilder.Append(elem_hash);
            }
            return sBuilder.ToString();
        }

        static string GetMd5Hash(MD5 hasher, string path)
        {
            byte[] data = hasher.ComputeHash(Encoding.UTF8.GetBytes(path));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i ++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
