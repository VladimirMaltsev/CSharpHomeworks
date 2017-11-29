using System;
using System.Collections.Generic;
using System.Net;

namespace HtmlParser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var stUri = Console.ReadLine();
            var parser = new HtmlParser();
            
            var pages = parser.GetHttpPagesDepth(new Page[]{new Page(stUri)}, 1).Result;
            foreach (var p in pages)
            {
                Console.WriteLine("Link: {0}   |>>    byte count = {1}", p.GetUri(), p.GetContent().Result.Length);
            }
            
            Console.ReadKey();
        }
        
        
    }
}