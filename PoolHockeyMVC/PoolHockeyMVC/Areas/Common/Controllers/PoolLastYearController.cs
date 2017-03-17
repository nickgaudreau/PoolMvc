using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PoolHockeyBLL.BizEntities;
using PoolHockeyBLL.Contracts;

namespace PoolHockeyMVC.Areas.Common.Controllers
{
    [RoutePrefix("2015-2016")]
    [RouteArea("Common")]
    [Route("{action}")]
    public class PoolLastYearController : BaseController
    {
        private readonly IPoolLastYearServices _poolLastYearServices;

        public PoolLastYearController(IPoolLastYearServices poolLastYearServices, IConfigServices configServices) : base(configServices)
        {
            _poolLastYearServices = poolLastYearServices;//new PoolLastYearServices();
        }

        [OutputCache(CacheProfile = "Long", VaryByHeader = "X-Requested-With;Accept-Language")]
        public ActionResult AllInOneLastYear()
        {
            try
            {
                // One DB query! ...but do not have proper display name so can't use in AllInOne without 2 queries
                var poolLastYear = _poolLastYearServices.GetAll().Where(x => x.L_Traded == false).ToList();

                if (!poolLastYear.Any())
                    return View("Error", new HandleErrorInfo(new NullReferenceException(), "PoolLastYear", "AllInOneLastYear"));


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
                return View("Error", new HandleErrorInfo(e, "PoolLastYear", "AllInOneLastYear"));
            }
        }
    }
}