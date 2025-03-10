﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using HtmlAgilityPack;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

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

                    //get images
                    var linkToItemHtml = webClient.DownloadString(linkToItem);
                    parser = new HtmlParser();
                    document = parser.ParseDocument(linkToItemHtml);
                    var itemImg = document.QuerySelectorAll(".swipe-wrap img").Select(m => m.GetAttribute("src")).FirstOrDefault();

                    List<Uri> imgList = new List<Uri>();
                    Uri uri = new Uri(itemImg);
                    imgList.Add(uri);

                    string txtMsg = $"{nameOfItem} in {cityOfItem} {distanceToItem} {linkToItem}";

                    string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
                    string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
                    TwilioClient.Init(accountSid, authToken);

                    var to = new PhoneNumber(Environment.GetEnvironmentVariable("MY_PHONE_NUMBER"));
                    var from = new PhoneNumber(Environment.GetEnvironmentVariable("TWILIO_PHONE_NUMBER"));

                    var message = MessageResource.Create(
                        to: to,
                        from: from,
                        body: txtMsg,
                        mediaUrl: imgList
                    );
                }
            }
        }
    }
}
