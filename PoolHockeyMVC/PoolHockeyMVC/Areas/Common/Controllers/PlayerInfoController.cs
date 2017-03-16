using System;
using System.Linq;
using System.Web.Mvc;
using PoolHockeyBLL.Contracts;

namespace PoolHockeyMVC.Areas.Common.Controllers
{
    [RoutePrefix("Stats")]
    [RouteArea("Common")]
    [Route("{action}")]
    public class PlayerInfoController : BaseController
    {
        private readonly IPlayerInfoServices _playerInfoServices;

        public PlayerInfoController(IPlayerInfoServices playerInfoServices, IConfigServices configServices) : base(configServices)
        {
            _playerInfoServices = playerInfoServices;
        }

        public ActionResult Undrafted(string sortOrder = "Asc", string sortBy = "")
        {
            try
            {
                var undrafted = _playerInfoServices.GetUndrafted();
                if (undrafted == null)
                    return View("Error", new HandleErrorInfo(new NullReferenceException(), "PlayerInfo", "Undrafted"));
                ViewBag.SortOrder = sortOrder;//(sortOrder == "Asc" ? "Desc" : "Asc");
                if(!string.IsNullOrEmpty(sortBy))
                    return View(SortUtility.SortPlayerInfoTable(undrafted, sortBy, sortOrder));

                return View(undrafted);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "PlayerInfo", "Undrafted"));
            }
        }

        // would be used with partial
        public ActionResult SortUndrafted(string sortOrder, string sortBy)
        {
            var undrafted = _playerInfoServices.GetUndrafted().ToList();
            ViewBag.SortOrder = sortOrder;
            //ViewBag.SortBy = sortBy;
            
            return PartialView("_undraftedGrid", SortUtility.SortPlayerInfoTable(undrafted, sortBy, sortOrder));
        }

        public ActionResult LeagueLeaders(string sortOrder = "Asc", string sortBy = "")
        {
            try
            {
                var leagueLeaders = _playerInfoServices.GetLeagueLeaders();
                if (leagueLeaders == null)
                    return View("Error", new HandleErrorInfo(new NullReferenceException(), "PlayerInfo", "LeagueLeaders"));
                ViewBag.SortOrder = sortOrder;//(sortOrder == "Asc" ? "Desc" : "Asc");
                if (!string.IsNullOrEmpty(sortBy))
                    return View(SortUtility.SortPlayerInfoTable(leagueLeaders, sortBy, sortOrder));

                return View(leagueLeaders);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "PlayerInfo", "LeagueLeaders"));
            }
        }

        public ActionResult Injured()
        {
            try
            {
                var injuredList = _playerInfoServices.GetInjured();

                if (injuredList == null)
                {
                    return View("Error", new HandleErrorInfo(new NullReferenceException(), "PlayerInfo", "Injured"));
                }
                return View(injuredList.OrderBy(x => x.C_Team));
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "PlayerInfo", "Undrafted"));
            }
        }

    }
}