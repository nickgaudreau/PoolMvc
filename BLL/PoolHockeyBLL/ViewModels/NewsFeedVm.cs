using System;

namespace PoolHockeyBLL.ViewModels
{
    public class NewsFeedVm
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public DateTime PubDate { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
    }
}
