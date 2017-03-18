using Newtonsoft.Json;

namespace UnitTestProject
{
    public class Issue
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
