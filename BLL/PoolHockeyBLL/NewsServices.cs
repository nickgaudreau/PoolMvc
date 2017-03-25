using System;
using System.Collections.Generic;
using PoolHockeyBLL.Api;
using PoolHockeyBLL.Contracts;
using PoolHockeyBLL.Log;
using PoolHockeyBLL.ViewModels;

namespace PoolHockeyBLL
{
    public class NewsServices : INewsServices
    {
        public IEnumerable<NewsFeedVm> GetItems()
        {
            var client = new Api.ApiXmlClient();
            var result = client.Get<PoolHockeyBLL.ApiModels.FR.rss>("http://rss.radio-canada.ca/fils", "/sports/hockey.xml"); // FR
            //var result = client.Get<PoolHockeyBLL.ApiModels.EN.rss>("http://www.cbc.ca/cmlink", "/rss-sports-nhl"); // ENG

            if (!result.Success || result.Data == null)
            {
                LogError.Write(new ApiException(result.Exception), $"News not retrieved: { result.ReasonPhrase}, code: {result.StatusCode}" );
                return null;
            }

            var vms = new List<NewsFeedVm>();
            foreach (var item in result.Data.channel.item)
            {
                // date form xml feed format: Sat, 18 Mar 2017 10:30:00 EDT
                var rssDate = item.pubDate;
                var dateTmzFixed = rssDate.Replace("EDT", "-0400");
                DateTime date = DateTime.ParseExact(dateTmzFixed, "ddd, dd MMM yyyy HH:mm:ss zzz",
                    System.Globalization.CultureInfo.InvariantCulture);
                vms.Add(new NewsFeedVm()
                {
                    Description = item.description,
                    ImgUrl = item.enclosure.url,
                    Title = item.title,
                    Link = item.link,
                    PubDate = date
                });
            }

            return vms;
        }
    }
}
