using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class NewsFeedTest
    {
        [TestMethod]
        public async Task Get_News_Feed_RadioCan_FR()
        {
            string result;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("http://rss.radio-canada.ca/fils/sports/hockey.xml");
                result = response.Content.ReadAsStringAsync().Result;
            }

            var serializer = new XmlSerializer(typeof(rss));
            rss feed;

            using (TextReader reader = new StringReader(result))
            {
                feed = (rss) serializer.Deserialize(reader);
            }
            var x = feed;

            XmlReader rssFeed = XmlReader.Create(new StringReader(result));
        }
    }
}
