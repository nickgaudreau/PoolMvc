using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PoolHockeyBLL.BizEntities;
using PoolHockeyBLL.Contracts;

namespace PoolHockeyMVC.Areas.Common.Controllers
{
    [RoutePrefix("Info")]
    [RouteArea("Common")]
    [Route("{action}")]
    public class UserInfoController : BaseController
    {

        private readonly IUserInfoServices _userInfoServices;
        private readonly IPlayerInfoServices _playerInfoServices;
        private readonly IPoolLastYearServices _poolLastYearServices;

        public UserInfoController(IPlayerInfoServices playerInfoServices, IUserInfoServices userInfoServices, 
            IPoolLastYearServices poolLastYearServices, IConfigServices configServices) : base(configServices)
        {
            _userInfoServices = userInfoServices;
            _playerInfoServices = playerInfoServices;
            _poolLastYearServices = poolLastYearServices;
        }

        [OutputCache(CacheProfile = "Long", VaryByHeader = "X-Requested-With;Accept-Language", VaryByParam = "*")]
        public ActionResult Table(string sortOrder = "Asc", string sortBy = ""/*, string dynamicSortOrder = "Asc", string dynamicSortBy = ""*/ )
        {
            try
            {
                var userInfoEntities = _userInfoServices.GetAll();
                if (userInfoEntities == null)
                    return View("Error", new HandleErrorInfo(new NullReferenceException(), "UserInfo", "Table"));

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
                return View("Error", new HandleErrorInfo(e, "UserInfo", "Table"));
            }
        }

        [OutputCache(CacheProfile = "Long", VaryByHeader = "X-Requested-With;Accept-Language")]
        public ActionResult AllInOne()
        {
            //_userInfoServices.UpdateAll();
            try
            {
                var users = _userInfoServices.GetAll().OrderByDescending(u => u.I_Points);
                ViewBag.OrderedUsers = users;

                var stats = new List<PlayerInfoEntity>();
                foreach (var user in users)
                {
                    stats.AddRange(_playerInfoServices.GetAllWhere(user.C_UserEmail));
                }

                return View(stats.OrderBy(p => p.I_Round));
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "UserInfo", "AllInOne"));
            }
        }

        [OutputCache(CacheProfile = "Long", VaryByHeader = "X-Requested-With;Accept-Language")]
        public ActionResult BestOfRound()
        {
            try
            {
                var bestOfRnd = _playerInfoServices.GetBestPerRound(1);
                var users = _userInfoServices.GetAll().ToList();

                foreach (var playerInfoEntity in bestOfRnd)
                {
                    var user = users.FirstOrDefault(u => u.C_UserEmail == playerInfoEntity.C_UserEmail);
                    if (user != null)
                    {
                        playerInfoEntity.C_UserEmail = user.C_DisplayName;
                    }
                }
                return View(bestOfRnd);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "UserInfo", "BestOfRound"));
            }
        }

        /// <summary>
        /// Partial round selector
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        [OutputCache(CacheProfile = "Long", VaryByHeader = "X-Requested-With;Accept-Language", VaryByParam = "*")]
        public ActionResult SetBestOfRound(int round)
        {
            var bestOfRnd = _playerInfoServices.GetBestPerRound(round);
            var users = _userInfoServices.GetAll().ToList();

            foreach (var playerInfoEntity in bestOfRnd)
            {
                var user = users.FirstOrDefault(u => u.C_UserEmail == playerInfoEntity.C_UserEmail);
                if (user != null)
                {
                    playerInfoEntity.C_UserEmail = user.C_DisplayName;
                }
            }
            return PartialView("_bestOfGrid", bestOfRnd);
        }

        [OutputCache(CacheProfile = "Long", VaryByHeader = "X-Requested-With;Accept-Language")]
        public ActionResult AllInOneLastYear()
        {
            try
            {
                // One DB query! ...but do not have proper display name so can't use in AllInOne without 2 queries
                var poolLastYear = _poolLastYearServices.GetAll().Where(x => x.L_Traded == false).ToList();

                if (!poolLastYear.Any())
                    return View("Error", new HandleErrorInfo(new NullReferenceException(), "UserInfo", "AllInOneLastYear"));


                var usersPoolLastYear = poolLastYear.Where(y => y.C_UserEmail.Length > 0).GroupBy(x => x.C_UserEmail).Select(x => x.Key);
                var userPointsDict = new Dictionary<string, int>();

                var stats = new List<PoolLastYearEntity>();

                foreach (var user in usersPoolLastYear)
                {
                    // Set dict for main loop
                    var points = poolLastYear.Where(x => x.C_UserEmail == user).Sum(p => p.I_Point);
                    userPointsDict.Add(user, points);

                    // set stats for inner loop
                    stats.AddRange(poolLastYear.Where(x => x.C_UserEmail == user));
                }

                ViewBag.OrderedUsers = userPointsDict.OrderByDescending(x => x.Value);

                return View(stats.OrderBy(p => p.I_Round));
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "UserInfo", "AllInOneLastYear"));
            }
        }

        [OutputCache(CacheProfile = "Long", VaryByHeader = "X-Requested-With;Accept-Language", VaryByParam = "*")]
        public ActionResult PoolerProfile(string userEmail, string displayName = "", string sortOrder = "Asc", string sortBy = "")
        {
            if (string.IsNullOrEmpty(userEmail))
                return View("Error", new HandleErrorInfo(new Exception("Malformed Request string param: userEmail"), "UserInfo", "PoolerProfile"));

            try
            {
                var poolerChoices = _playerInfoServices.GetAllWhere(userEmail);
                if (poolerChoices == null)
                    return View("Error", new HandleErrorInfo(new NullReferenceException(), "UserInfo", "PoolerProfile"));

                var userInfo = _userInfoServices.GetByEmail(userEmail);
                if (userInfo == null)
                    return View("Error", new HandleErrorInfo(new NullReferenceException(), "UserInfo", "PoolerProfile"));


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