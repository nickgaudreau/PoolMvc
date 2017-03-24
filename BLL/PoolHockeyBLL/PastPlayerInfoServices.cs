using System;
using System.Collections.Generic;
using System.Linq;
using PoolHockeyBLL.BizEntities;
using PoolHockeyBLL.Contracts;
using PoolHockeyBLL.Log;
using PoolHockeyBOL;
using PoolHockeyDAL.UnitOfWork;

namespace PoolHockeyBLL
{
    /// <summary>
    /// No caching needed, just get/set on daily routine
    /// </summary>
    public class PastPlayerInfoServices : IPastPlayerInfoServices
    {
        private IUnitOfWork _unitOfWork;

        ///// <summary>
        ///// For  internal instantiation 
        ///// </summary>
        //internal PastPlayerInfoServices() { }

        // DI
        public PastPlayerInfoServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; //new UnitOfWork();
        }

        public int GetYesterdayWhere(PlayerInfoEntity playerInfoEntity)
        {
            //var lastSeasonLastDate = new DateTime(2016, 03, 21);
            var date = DateTime.Now.AddDays(-1);//lastSeasonLastDate.AddDays(-1);//DateTime.Now.AddDays(-1);
            var pastPlayerInfo = _unitOfWork.PastPlayerInfoRepository.GetFirst(x =>
                x.D_Date.Day == date.Day && x.D_Date.Month == date.Month &&
                x.I_ApiId == playerInfoEntity.I_ApiId);
                       //x.C_Name == playerInfoEntity.C_Name && x.C_Team == playerInfoEntity.C_Team && x.C_Pos == playerInfoEntity.C_Pos);
            if (pastPlayerInfo.Result == null)
                return 0;

            return (pastPlayerInfo.Result.I_Point);
        }

        public int GetWeekWhere(PlayerInfoEntity playerInfoEntity)
        {
            //var lastSeasonLastDate = new DateTime(2016, 03, 21);
            var allPastDatForApiId =
                _unitOfWork.PastPlayerInfoRepository.GetManyQueryable(x => x.I_ApiId == playerInfoEntity.I_ApiId );//x.C_Name == playerInfoEntity.C_Name && x.C_Team == playerInfoEntity.C_Team && x.C_Pos == playerInfoEntity.C_Pos).ToList();

            var totalLastWeek = 0;

            var date = DateTime.Now.AddDays(-7);//lastSeasonLastDate.AddDays(-7);//DateTime.Now.AddDays(-1);

            for (var d = date; d < DateTime.Today; d = d.AddDays(1))
            {
                var pastPlayerInfo = allPastDatForApiId.FirstOrDefault(x =>
                       x.D_Date.Day == d.Day && x.D_Date.Month == d.Month &&
                       x.I_ApiId == playerInfoEntity.I_ApiId);
                //x.C_Name == playerInfoEntity.C_Name && x.C_Team == playerInfoEntity.C_Team && x.C_Pos == playerInfoEntity.C_Pos);
                if (pastPlayerInfo == null) continue;
                totalLastWeek += pastPlayerInfo.I_Point;
            }

            //if (pastPlayerInfo == null)
            //    return 0;
            //if (currentPts == pastPlayerInfo.I_Point)
            //    return currentPts;
            return totalLastWeek; //(currentPts - pastPlayerInfo.I_Point);
        }

        public int GetMonthWhere(PlayerInfoEntity playerInfoEntity)
        {
            var allPastDatForApiId = _unitOfWork.PastPlayerInfoRepository
                .GetManyQueryable(x => x.I_ApiId == playerInfoEntity.I_ApiId);//x => x.C_Name == playerInfoEntity.C_Name && x.C_Team == playerInfoEntity.C_Team && x.C_Pos == playerInfoEntity.C_Pos).ToList();

            //var lastSeasonLastDate = new DateTime(2016, 03, 21);
            var totalLastMonth = 0;

            var date = DateTime.Now.AddMonths(-1);//lastSeasonLastDate.AddMonths(-1);//DateTime.Now.AddDays(-1);

            // To get actual month - use below var in loop => d= dateDay1CurrentMonth - but watchout break 30j logic
            //var dateDay1CurrentMonth = new DateTime(dateNow.Year, dateNow.Month, 01);

            for (var d = date; d < DateTime.Today; d = d.AddDays(1))
            {
                var pastPlayerInfo = allPastDatForApiId.FirstOrDefault(x =>
                       x.D_Date.Day == d.Day && x.D_Date.Month == d.Month &&
                        x.I_ApiId == playerInfoEntity.I_ApiId);
                //x.C_Name == playerInfoEntity.C_Name && x.C_Team == playerInfoEntity.C_Team && x.C_Pos == playerInfoEntity.C_Pos);
                if (pastPlayerInfo == null) continue;
                totalLastMonth += pastPlayerInfo.I_Point;
            }
            //if (pastPlayerInfo == null)
            //    return 0;
            //if (currentPts == pastPlayerInfo.I_Point)
            //    return currentPts;
            return totalLastMonth;
        }

        public int GetActualMonthWhere(PlayerInfo playerInfoEntity) // todo change to playerInfoEntity or DTO
        {
            var allPastDatForApiId = _unitOfWork.PastPlayerInfoRepository
                .GetManyQueryable(x => x.I_ApiId == playerInfoEntity.I_ApiId);//x => x.C_Name == playerInfoEntity.C_Name && x.C_Team == playerInfoEntity.C_Team && x.C_Pos == playerInfoEntity.C_Pos).ToList();

            var totalLastMonth = 0;

            var dateYesterday = DateTime.Now.AddDays(-1);

            // To get actual month - use below var in loop => d= dateDay1CurrentMonth - but watchout break 30j logic
            var dateDay1CurrentMonth = new DateTime(dateYesterday.Year, dateYesterday.Month, 01);

            for (var d = dateDay1CurrentMonth; d < DateTime.Today; d = d.AddDays(1))
            {
                var pastPlayerInfo = allPastDatForApiId.FirstOrDefault(x =>
                       x.D_Date.Day == d.Day && x.D_Date.Month == d.Month &&
                       x.I_ApiId == playerInfoEntity.I_ApiId);
                //x.C_Name == playerInfoEntity.C_Name && x.C_Team == playerInfoEntity.C_Team && x.C_Pos == playerInfoEntity.C_Pos);
                if (pastPlayerInfo == null) continue;
                totalLastMonth += pastPlayerInfo.I_Point;
            }
            return totalLastMonth;
        }

        /// <summary>
        ///  Jan 21st - this should never un m ore that daily... otherwise half updated data for a player
        /// lets say at 12h01pm, playerXyz has 1 pt, then end of game playerXyz has 2 pts - update try to add new yesterday for playerXyz with 
        /// 2 pts but can't since already exist
        /// </summary>
        /// <param name="playerInfoEntities"></param>
        /// <returns></returns>
        public bool Create(IEnumerable<PlayerInfoEntity> playerInfoEntities)
        {
            var dateToStore = DateTime.Now.AddDays(-1);

            var listPlayerInfoForTeam = _unitOfWork.PlayerInfoRepository.GetManyQueryable(p => p.C_Team == playerInfoEntities.ElementAt(0).C_Team);

            var listPastPlayerInfoYesterdayForTeam =
                _unitOfWork.PastPlayerInfoRepository.GetManyQueryable(x => x.D_Date == dateToStore && x.C_Team == playerInfoEntities.ElementAt(0).C_Team);


            foreach (var playerInfoEntity in playerInfoEntities)
            {
                // list with param team check if apiId is in with data for yesterday if so skip (coded that way in case 2 update would occur in one day)
                var exist =
                    listPastPlayerInfoYesterdayForTeam.FirstOrDefault(x => x.I_ApiId == playerInfoEntity.I_ApiId);
                //var exist =
                //    listPastPlayerInfoYesterdayForTeam
                //    .FirstOrDefault(x => x.C_Name == playerInfoEntity.C_Name && x.C_Team == playerInfoEntity.C_Team && x.C_Pos == playerInfoEntity.C_Pos);
                if (exist != null) continue;

                var playerInfo = listPlayerInfoForTeam.FirstOrDefault(p => p.I_ApiId == playerInfoEntity.I_ApiId);
                //var playerInfo = listPlayerInfoForTeam
                //    .FirstOrDefault(x => x.C_Name == playerInfoEntity.C_Name && x.C_Team == playerInfoEntity.C_Team && x.C_Pos == playerInfoEntity.C_Pos);
                if (playerInfo == null)
                    continue;

                try
                {
                    var lastDayPoints = playerInfoEntity.I_Point - playerInfo.I_Point;

                    if (lastDayPoints <= 0) // no need to store 0 points... when geet foir stats if null = skip or return 0 anyway
                        continue;

                    if (playerInfoEntities.ElementAt(0).C_Team == "PIT")
                    {
                        var x = "";
                    }

                    var pastPlayerInfo = new PastPlayerInfo()
                    {
                        C_Name = playerInfo.C_Name,
                        C_Pos = playerInfo.C_Pos,
                        C_Team = playerInfo.C_Team,
                        D_Date = dateToStore,
                        I_ApiId = playerInfo.I_ApiId,
                        I_Point = lastDayPoints, //playerInfoFromDb.I_Point, 
                        L_Traded = playerInfo.L_Traded
                    };
                    _unitOfWork.PastPlayerInfoRepository.Insert(pastPlayerInfo);
                }
                catch (ApplicationException ex)
                {
                    LogError.Write(ex,
                        $"ApplicationException in PastPlayerInfoServices.Create(IEnumerable<PlayerInfoEntity> playerInfoEntities)() for id: {playerInfo.C_Name} and code: {playerInfo.C_Team}");
                }
                catch (Exception ex)
                {
                    LogError.Write(ex,
                        $"Exception in PastPlayerInfoServices.Create(IEnumerable<PlayerInfoEntity> playerInfoEntities)() for id: {playerInfo.C_Name} and code: {playerInfo.C_Team}");
                }
            }

            // Do the save changes after all done - should never be more than 125 or so - no its by team
            try
            {
                _unitOfWork.Save();
                _unitOfWork = new UnitOfWork();
            }
            catch (Exception e)
            {
                LogError.Write(e,
                        "Exception in PastPlayerInfoServices.Create(IEnumerable<PlayerInfoEntity> playerInfoEntities)()  on Save to Context");
                return false;
            }

            return true;
        }

        public bool Exist(PlayerInfo playerInfo)
        {
            var now = DateTime.Now;
            var exist = _unitOfWork.PastPlayerInfoRepository
                .GetFirst(
                    x => x.C_Name == playerInfo.C_Name && x.C_Team == playerInfo.C_Team && x.C_Pos == playerInfo.C_Pos && x.D_Date.Day == now.Day && x.D_Date.Month == now.Month);
            if (exist.Result == null)
                return false;
            return true;
        }
    }
}
