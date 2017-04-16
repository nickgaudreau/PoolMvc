using System;
using System.Linq;
using System.Web.Mvc;
using PoolHockeyBLL.Contracts;

namespace PoolHockeyMVC.Areas.Common.Controllers
{
    [RoutePrefix("PlayoffInfo")]
    [RouteArea("Common")]
    [Route("{action}")]
    public class PlayoffUserInfoController : BaseController
    {

        private readonly IPlayoffUserInfoServices _userInfoServices;
        private readonly IPlayoffPlayerInfoServices _playerInfoServices;

        public PlayoffUserInfoController(IPlayoffPlayerInfoServices playerInfoServices, IPlayoffUserInfoServices userInfoServices, IConfigServices configServices) : base(configServices)
        {
            _userInfoServices = userInfoServices;
            _playerInfoServices = playerInfoServices;
        }

        [OutputCache(CacheProfile = "Long", VaryByHeader = "X-Requested-With;Accept-Language", VaryByParam = "*")]
        public ActionResult Table(string sortOrder = "Asc", string sortBy = ""/*, string dynamicSortOrder = "Asc", string dynamicSortBy = ""*/ )
        {
            try
            {
                var userInfoEntities = _userInfoServices.GetAll();
                if (userInfoEntities == null)
                    return View("Error", new HandleErrorInfo(new NullReferenceException(), "PlayoffUserInfo", "Table"));

                userInfoEntities = userInfoEntities.OrderByDescending(u => u.I_Points);
                userInfoEntities = userInfoEntities.ToList(); // avoid multiple enumeration

                ViewBag.BestDay = _userInfoServices.GetTopBestDay();
                ViewBag.BestMonth = _userInfoServices.GetTopBestMonth();
                ViewBag.BestPtsLastD = userInfoEntities.OrderByDescending(u => u.I_PtLastD).First();

                ViewBag.SortOrder = sortOrder;//(sortOrder == "Asc" ? "Desc" : "Asc");
                if (!string.IsNullOrEmpty(sortBy))
                    return View(SortUtility.SortUserInfoTable(userInfoEntities, sortBy, sortOrder));

                return View(userInfoEntities);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "PlayoffUserInfo", "Table"));
            }
        }

        [OutputCache(CacheProfile = "Long", VaryByHeader = "X-Requested-With;Accept-Language", VaryByParam = "*")]
        public ActionResult PoolerProfile(string userEmail, string displayName = "", string sortOrder = "Asc", string sortBy = "")
        {
            if (string.IsNullOrEmpty(userEmail))
                return View("Error", new HandleErrorInfo(new Exception("Malformed Request string param: userEmail"), "PlayoffUserInfo", "PoolerProfile"));

            try
            {
                var poolerChoices = _playerInfoServices.GetAllWhere(userEmail);
                if (poolerChoices == null)
                    return View("Error", new HandleErrorInfo(new NullReferenceException(), "PlayoffUserInfo", "PoolerProfile"));

                var userInfo = _userInfoServices.GetByEmail(userEmail);
                if (userInfo == null)
                    return View("Error", new HandleErrorInfo(new NullReferenceException(), "PlayoffUserInfo", "PoolerProfile"));


                ViewBag.DisplayName = displayName;
                ViewBag.UserEmail = userEmail;
                ViewBag.Pic = userInfo.C_Pic;
                ViewBag.BestDay = userInfo.I_BestDay;
                ViewBag.BestDayDate = userInfo.D_BestDay;
                ViewBag.BestMonth = userInfo.I_BestMonth;
                ViewBag.BestMonthDate = userInfo.D_BestMonth;
                ViewBag.PtsLastD = userInfo.I_PtLastD;

                //ViewBag.UserInfo = userInfo;

                ViewBag.SortOrder = sortOrder;//(sortOrder == "Asc" ? "Desc" : "Asc");
                if (!string.IsNullOrEmpty(sortBy))
                    return View(SortUtility.SortPlayerInfoTable(poolerChoices, sortBy, sortOrder));

                return View(poolerChoices.OrderBy(p => p.I_Round));
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "PlayerInfo", "Undrafted"));
            }
        }

    }
}