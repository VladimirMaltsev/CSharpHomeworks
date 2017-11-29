using System;
using System.Net;
using System.Threading.Tasks;

namespace HtmlParser
{
    public class Page
    {
        private readonly Uri uri;

        public Page(string link)
        {
            uri = new Uri(link);
        }
        public Uri GetUri()
        {
            return uri;
        }

        public async Task<String> GetContent()
        {
            String html = "";
            using (WebClient client = new WebClient())
            {
                try
                {
                    html = await client.DownloadStringTaskAsync(uri);
                }
                catch (Exception ex)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write("(!) Exception >> {1} {0}", uri, ex.Message);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }
                
            }
            return html;
        }
    }
}