using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoolHockeyBLL.Contracts;
using PoolHockeyBLL.ViewModels;

namespace PoolHockeyMVC.Areas.Common.Controllers
{
    [RoutePrefix("News")]
    [RouteArea("Common")]
    [Route("{action}")]
    public class NewsController : BaseController
    {
        private readonly INewsServices _newsServices;

        public NewsController(INewsServices newsServices, IConfigServices configServices) : base(configServices)
        {
            _newsServices = newsServices;
        }

        // GET: Common/News
        [OutputCache(CacheProfile = "Long", VaryByHeader = "X-Requested-With;Accept-Language")]
        public ActionResult Index()
        {
            var data = _newsServices.GetItems();//new List<NewsFeedVm>() {new NewsFeedVm() {Description = "test"} };//
            if(data != null)
                return View(data.OrderByDescending(x => x.PubDate).Take(5));

            var tmpData = new List<NewsFeedVm>();
            for (int i = 0; i < 5; i++)
            {
                tmpData.Add(new NewsFeedVm()
                {
                    Description = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. Lorem ipsum dolor sit amet, consetetur sadipscing elitr",
                    ImgUrl = "http://placehold.it/760x400/999999/cccccc",
                    Link = "#",
                    Title = "Lorem ipsum dolor sit amet consetetur sadipscing",
                    PubDate = DateTime.Now
                });
            }

            return View(tmpData);
        }
    }
}