using System;
using System.Web.Mvc;
using PoolHockeyBLL.Contracts;

namespace PoolHockeyMVC.Areas.Common.Controllers
{
    [RoutePrefix("Home")]
    [RouteArea("Common")]
    [Route("{action}")]
    public class HomeController : BaseController
    {
        //private readonly IUserInfoServices _userInfoServices;

        //public HomeController(IUserInfoServices userInfoServices, IConfigServices configServices) : base(configServices)
        //{
        //    _userInfoServices = userInfoServices; //new UserInfoServices();
        //}
        
        // GET: Common/Home
        [Route("~/")]
        [Route]
        [OutputCache(CacheProfile = "Long", VaryByHeader = "X-Requested-With;Accept-Language")]
        public ActionResult Index()
        {
            try
            {
                //var userInfoEntities = _userInfoServices.GetAll().OrderByDescending(u => u.I_Points);
                //return View(userInfoEntities);
                return View();
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Home", "Index"));
            }
            
        }

        
        public ActionResult LoginRedirect()
        {
            return RedirectToRoute("login");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LogOffRedirect()
        //{
        //    return RedirectToRoute("logout");
        //}

        public ActionResult RegisterRedirect()
        {
            return RedirectToRoute("register");
        }

    }


}