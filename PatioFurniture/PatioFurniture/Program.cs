using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using HtmlAgilityPack;

namespace PatioFurniture
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var webClient = new WebClient())
            {
                var craigsListHtml = webClient.DownloadString("https://losangeles.craigslist.org/search/zip?sort=date&postal=90305&query=patio%20furniture&search_distance=25");
                var parser = new HtmlParser();
                var document = parser.ParseDocument(craigsListHtml);
                var resultRows = document.QuerySelectorAll(".result-row");
                List<string> linksToItems = new List<string>();

                foreach (var resultRow in resultRows)
                {
                    var nameOfItem = resultRow.QuerySelector(".result-title.hdrlnk").TextContent;
                    var linkToItem = resultRow.QuerySelectorAll(".result-title.hdrlnk").Select(m => m.GetAttribute("href")).FirstOrDefault();
                    var cityOfItem = resultRow.QuerySelector(".result-hood").TextContent;
                    var distanceToItem = resultRow.QuerySelector(".maptag").TextContent;
                    Console.WriteLine($"Name: {nameOfItem}");
                    Console.WriteLine($"Link: {linkToItem}");
                    Console.WriteLine($"City: {cityOfItem}");
                    Console.WriteLine($"Distance: {distanceToItem}");

                    //linksToItems.Add(linkToItem.FirstOrDefault());

                    //get images
                    var linkToItemHtml = webClient.DownloadString(linkToItem);
                    parser = new HtmlParser();
                    document = parser.ParseDocument(linkToItemHtml);
                    var itemImg = document.QuerySelectorAll(".swipe-wrap img").Select(m => m.GetAttribute("src")).FirstOrDefault();
                    Console.WriteLine($"Img: {itemImg}");
                }

                //get images
                //foreach (string link in linksToItems)
                //{
                //    var linkToItemHtml = webClient.DownloadString(link);
                //    parser = new HtmlParser();
                //    document = parser.ParseDocument(linkToItemHtml);
                //    var itemImg = document.QuerySelectorAll(".swipe-wrap img").Select(m => m.GetAttribute("src")).FirstOrDefault();
                //    Console.WriteLine($"Img: {itemImg}");
                //}
            }
            Console.WriteLine("");
        }
    }
}
