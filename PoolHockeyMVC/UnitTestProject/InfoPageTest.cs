using System.Collections.Generic;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class InfoPageTest
    {
        [TestMethod]
        public void Get_GitHub_Commits_For_Info_Page()
        {
            var apiLink = "https://api.github.com/repos/nickgaudreau/PoolMvc/commits";

            var deserCommits = new List<Commit>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

                using (var response = client.GetAsync(apiLink).Result)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    dynamic commits = JArray.Parse(json);
                    foreach (var data in commits)
                    {
                        deserCommits.Add(new Commit()
                        {
                            Message = data.commit.message,
                            Date = data.commit.author.date
                        });
                    }
                    
                }
            }
        }

        [TestMethod]
        public void Post_GitHub_Issues_For_Info_Page()
        {
            var issue = new Issue() {Title = "New issue from Web app by user", Body = "This a new issue sent from unit test"};
            var client = new WebApiClient();
            var result = client.Post("https://api.github.com", "/repos/nickgaudreau/PoolMvc/issues", issue);
        }



    }
}
