using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlParser
{
    public class HtmlParser
    {
        public async Task<Page[]> GetHttpPagesDepth(Page[] ps, int depth)
        {
            while (true)
            {
                foreach (var p in ps)
                {
                    var innerPagesP = await ParseHttpPageAsync(p);
                    ps = ps.Concat(innerPagesP).ToArray();
                }
                if (depth < 1) 
                    return ps;
                depth = depth - 1;
            }
        }

        private async Task<Page[]> ParseHttpPageAsync(Page p)
        {
            var httpPattern = new Regex(@"<a.*? href=""(?<url>((https?:\/\/)([\w\.]+)\.([a-z]{2,6}\.?)(\/[\w\.]*)*\/?))");
            var html = await p.GetContent();
            var links = httpPattern.Matches(html);

            var pages = new Page[links.Count];
            for (var i = 0; i < links.Count; i++)
            {
                pages[i] = new Page(links[i].Groups["url"].Value);
            }
            return pages;
        } 
    }
}