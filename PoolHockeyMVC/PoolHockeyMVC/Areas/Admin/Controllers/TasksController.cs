using System;
using System.Diagnostics;
using System.Web.Mvc;
using PoolHockeyBLL;
using PoolHockeyBLL.Api;
using PoolHockeyBLL.Contracts;

namespace PoolHockeyMVC.Areas.Admin.Controllers
{
    [RoutePrefix("Tasks")]
    [RouteArea("Admin")]
    [Route("{action}")]
    public class TasksController : Controller
    {
        private readonly IUserInfoServices _userInfoServices;
        private readonly IPlayerInfoServices _playerInfoServices;
        private readonly IPastPlayerInfoServices _pastPlayerInfoServices;
        private readonly ITeamScheduleServices _teamScheduleServices;
        private readonly IConfigServices _configServices;

        public TasksController(IUserInfoServices userInfoServices, IPlayerInfoServices playerInfoServices, IPastPlayerInfoServices pastPlayerInfoServices, ITeamScheduleServices teamScheduleServices, IConfigServices configServices)
        {
            _userInfoServices = userInfoServices;//new UserInfoServices();
            _playerInfoServices = playerInfoServices;//new PlayerInfoServices();
            _pastPlayerInfoServices = pastPlayerInfoServices;
            _teamScheduleServices = teamScheduleServices;//new TeamScheduleServices();
            _configServices = configServices;//new ConfigServices();
        }

        public ActionResult ClearCache()
        {
            // CLear all chaches
            Caching.ClearAllCaches();

            return RedirectToAction("Index", "Home", new { Area = "Common" });
        }

        /// <summary>
        /// One off. Create / fill PlayerInfo - dangerous
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult CreateAllPlayerInfo()
        {
            Caching.ClearAllCaches();
            var timer = Stopwatch.StartNew();
            var api = new MySportsFeedApiTransactions(_playerInfoServices, _pastPlayerInfoServices);
            var data = api.SplitDataTeamLists();

            // Create PlayerInfo table team per team
            foreach (var playerInfoList in data)
                _playerInfoServices.CreateList(playerInfoList);

            Caching.ClearAllCaches();
            // for DEBUG only
            //Debug.WriteLine("CreateAllPlayerInfo time: " + timer.Elapsed);
            // For PROD only
            SendMessage("DailyPastStatsFiller", "Success", timer.Elapsed.ToString()); // fail will be sent by LogError
            return RedirectToAction("Index", "Home", new { Area = "Common" });
        }
        /// <summary>
        /// One off.Player status and user cumulative
        /// </summary>
        /// <returns></returns>
        public ActionResult PlayerUserStatsUpdater()
        {
            Caching.ClearAllCaches();
            var timer = Stopwatch.StartNew();

            //Update player info
            _playerInfoServices.UpdateStatus();
            _playerInfoServices.UpdateAvg();
            _playerInfoServices.UpdateInjuryStatus();

            // Update UserIfo Table
            _userInfoServices.UpdateAll();
            _userInfoServices.UpdateBestMonth();
            _userInfoServices.UpdateBestDay(); //// best day must be call last after all other updates

            // Update TeamSchedule table -> who play today
            _teamScheduleServices.Update();

            // Udpate Config Table - set last update time -> NOW
            _configServices.SetLastUpdate(DateTime.Now);

            Caching.ClearAllCaches();
            // for DEBUG only
            //Debug.WriteLine("PlayerUserStatsUpdater time: " + timer.Elapsed);
            // For PROD only
            SendMessage("FrequentPlayerUserStatsUpdater", "Success", timer.Elapsed.ToString()); // fail will be sent by LogError
            return RedirectToAction("Index", "Home", new { Area = "Common" });
        }

        /// <summary>
        /// Past Player Info Filler. Run daily with data from MySportsFeeds Api.
        /// Now sperated from other rtoutine, 1st cuz of speration of concerns, 2 the main routine now run every 2 hours, 3 will make the main routine lighter ( -lt 5 min)
        /// </summary>
        /// <returns></returns>
        public ActionResult DailyPastStatsFiller()
        {
            Caching.ClearAllCaches();
            var timer = Stopwatch.StartNew();
            var api = new MySportsFeedApiTransactions(_playerInfoServices, _pastPlayerInfoServices);
            var data = api.SplitDataTeamLists();

            // Update PastPlayerInfo table team per team
            foreach (var playerInfoList in data)
                _pastPlayerInfoServices.Create(playerInfoList);

            Caching.ClearAllCaches();
            // for DEBUG only
            //Debug.WriteLine("DailyPastStatsFiller time: " + timer.Elapsed);
            // For PROD only
            SendMessage("DailyPastStatsFiller", "Success", timer.Elapsed.ToString()); // fail will be sent by LogError
            return RedirectToAction("Index", "Home", new { Area = "Common" });
        }

        /// <summary>
        /// Create PastPlayerInfo and update PlayerInfo. Then RUn user info routines, injury routine and team sched routine.
        /// </summary>
        /// <returns></returns>
        public ActionResult DailyFullUpdater()
        {
            Caching.ClearAllCaches();
            var timer = Stopwatch.StartNew();
            var api = new MySportsFeedApiTransactions(_playerInfoServices, _pastPlayerInfoServices);
            var data = api.SplitDataTeamLists();

            // Update PlayerInfo table team per team
            foreach (var playerInfoList in data)
            {
                _pastPlayerInfoServices.Create(playerInfoList);
                _playerInfoServices.Update(playerInfoList);
            }
            _playerInfoServices.UpdateStatus();
            _playerInfoServices.UpdateAvg();
            _playerInfoServices.UpdateInjuryStatus();

            // Update UserIfo Table
            _userInfoServices.UpdateAll();
            _userInfoServices.UpdateBestMonth();
            _userInfoServices.UpdateBestDay(); //// best day must be call last after all other updates

            // Update TeamSchedule table -> who play today
            _teamScheduleServices.Update();

            // Udpate Config Table - set last update time -> NOW
            _configServices.SetLastUpdate(DateTime.Now);

            Caching.ClearAllCaches();
            // for DEBUG only
            //Debug.WriteLine("DailyFullUpdater time: " + timer.Elapsed);
            // For PROD only
            SendMessage("DailyFullUpdater", "Success", timer.Elapsed.ToString()); // fail will be sent by LogError
            return RedirectToAction("Index", "Home", new { Area = "Common" });
        }

        public ActionResult FrequentPlayerUserStatsUpdater()
        {
            Caching.ClearAllCaches();
            var timer = Stopwatch.StartNew();
            var api = new MySportsFeedApiTransactions(_playerInfoServices, _pastPlayerInfoServices);
            var data = api.SplitDataTeamLists();

            // Update PlayerInfo table team per team
            foreach (var playerInfoList in data)
                _playerInfoServices.Update(playerInfoList);
            _playerInfoServices.UpdateStatus();
            _playerInfoServices.UpdateAvg();
            //_playerInfoServices.UpdateInjuryStatus(); // no need to be frequent

            // Update UserIfo Table
            _userInfoServices.UpdateAll();
            //_userInfoServices.UpdateBestMonth(); // no need to be frequent
            //_userInfoServices.UpdateBestDay(); // no need to be frequent // best day must be call last after all other updates

            // Update TeamSchedule table -> who play today
            //_teamScheduleServices.Update(); // no need to be frequent - daily

            // Udpate Config Table - set last update time -> NOW
            _configServices.SetLastUpdate(DateTime.Now);

            Caching.ClearAllCaches();
            // For PROD only
            SendMessage("FrequentPlayerUserStatsUpdater", "Success", timer.Elapsed.ToString()); // fail will be sent by LogError
            return RedirectToAction("Index", "Home", new { Area = "Common" });
        }

        public void SendMessage(string cronType, string status, string addToMessage)
        {
            var message = $"{cronType} : {status}. Total time: {addToMessage}";
            var subject = $"{cronType} - {status}";
            MailUtility.SendMail(message, subject);
        }


    }
}