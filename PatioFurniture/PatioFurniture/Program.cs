using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace PatioFurniture
{
    class Program
    {
        static void Main(string[] args)
        {
            var webClient = new WebClient();
            var html = webClient.DownloadString("https://losangeles.craigslist.org/search/zip?query=patio+furniture&search_distance=5&postal=90305");

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var nodes = htmlDoc
                .DocumentNode
                .Descendants()
                .Where(node =>
                    node.Attributes["class"] != null &&
                    node.Attributes["class"].Value.Contains("result-row"));

            foreach (var node in nodes)
            {
                Console.WriteLine(node.InnerHtml);
            }

            Console.WriteLine(html);
        }
    }
}
