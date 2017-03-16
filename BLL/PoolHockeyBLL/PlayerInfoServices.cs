using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using AutoMapper;
using PoolHockeyBLL.Api;
using PoolHockeyBLL.BizEntities;
using PoolHockeyBLL.Contracts;
using PoolHockeyBLL.Log;
using PoolHockeyBOL;
using PoolHockeyDAL.UnitOfWork;

namespace PoolHockeyBLL
{
    
    public class PlayerInfoServices : IPlayerInfoServices
    {
        private IUnitOfWork _unitOfWork; // not readonly to be able to dispose and create new context during split bulk save changes
        private readonly IPastPlayerInfoServices _pastPlayerInfoServices;
        //private readonly ITeamScheduleServices _teamScheduleServices;
        private readonly ICaching _caching;

        ///// <summary>
        ///// For internal use
        ///// </summary>
        //internal PlayerInfoServices() { }
        
        public PlayerInfoServices(UnitOfWork unitOfWork, IPastPlayerInfoServices pastPlayerInfoServices)
        {
            _unitOfWork = unitOfWork;//new UnitOfWork();
            _pastPlayerInfoServices = pastPlayerInfoServices;//new PastPlayerInfoServices();
            //_teamScheduleServices = new TeamScheduleServices();
            _caching = new Caching();
        }

        /// <summary>
        /// Fetches PlayerInfo details by code
        /// </summary>
        /// <param name="playerInfoCode"></param>
        /// <returns></returns>
        public PlayerInfoEntity GetById(string playerInfoCode)
        {
            var playerInfo = _unitOfWork.PlayerInfoRepository.GetByID(playerInfoCode);            if (playerInfo == null) return null;

            Mapper.CreateMap<PlayerInfo, PlayerInfoEntity>();
            var playerInfoEntity = Mapper.Map<PlayerInfo, PlayerInfoEntity>(playerInfo);

            return playerInfoEntity;
        }

        public IEnumerable<PlayerInfoEntity> GetBestPerRound(int round)
        {
            var playerInfoCache = (List<PlayerInfo>)_caching.GetCachedItem("PlayerInfoGetAll");

            List<PlayerInfo> playerInfo = null; // This one ok as a list since it is not .Tolist() again
            if (playerInfoCache == null)
            {
                playerInfo = _unitOfWork.PlayerInfoRepository.GetAll().ToList();
                _caching.AddToCache("PlayerInfoGetAll", playerInfo);
                if (!playerInfo.Any()) return null;
            }
            else
            {
                playerInfo = playerInfoCache;
            }

            var playerInfoOrderedByRound = playerInfo
                .Where(x => x.I_Round == round && x.L_Traded == false && x.C_UserEmail != String.Empty)
                .OrderByDescending(p => p.I_Point)
                .ThenBy(g => g.I_Game).ToList();

            Mapper.CreateMap<PlayerInfo, PlayerInfoEntity>();
            var playerInfoEntities = Mapper.Map<List<PlayerInfo>, List<PlayerInfoEntity>>(playerInfoOrderedByRound);

            return playerInfoEntities;
        }

        /// <summary>
        /// Fetches all PlayerInfo
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PlayerInfoEntity> GetAll()
        {
            var playerInfos = _unitOfWork.PlayerInfoRepository.GetAll().ToList();            if (!playerInfos.Any()) return null; // TODO sghould also log

            Mapper.CreateMap<PlayerInfo, PlayerInfoEntity>();
            var playerInfoEntities = Mapper.Map<List<PlayerInfo>, List<PlayerInfoEntity>>(playerInfos);

            return playerInfoEntities;
        }

        public IEnumerable<PlayerInfoEntity> GetUndrafted()
        {
            var playerInfoCache = (List<PlayerInfo>)_caching.GetCachedItem("PlayerInfoGetAll");

            List<PlayerInfo> playerInfo = null; // This one ok as a list since it is not .Tolist() again
            if (playerInfoCache == null)
            {
                playerInfo = _unitOfWork.PlayerInfoRepository.GetAll().ToList();
                _caching.AddToCache("PlayerInfoGetAll", playerInfo);
                if (!playerInfo.Any()) return null;
            }
            else
            {
                playerInfo = playerInfoCache;
            }

            var allWhere = playerInfo.Where(p => p.C_UserEmail == String.Empty && p.L_Traded == false).ToList();            if (!allWhere.Any())
            {
                LogError.Write(new Exception("PlayerInfoServices"), "GetUndrafted returned 0");
                return null; // TODO sghould also log
            }

            var playerInfoWhere = allWhere
                .OrderByDescending(z => z.I_Point)
                .ThenBy(y => y.I_Game)
                .Take(50).ToList();

            Mapper.CreateMap<PlayerInfo, PlayerInfoEntity>();
            var playerInfoEntities = Mapper.Map<List<PlayerInfo>, List<PlayerInfoEntity>>(playerInfoWhere);

            return playerInfoEntities;
        }

        public IEnumerable<PlayerInfoEntity> GetLeagueLeaders()
        {
            var playerInfoCache = (List<PlayerInfo>)_caching.GetCachedItem("PlayerInfoGetAll");

            List<PlayerInfo> playerInfo = null; // This one ok as a list since it is not .Tolist() again
            if (playerInfoCache == null)
            {
                playerInfo = _unitOfWork.PlayerInfoRepository.GetAll().ToList();
                _caching.AddToCache("PlayerInfoGetAll", playerInfo);
                if (!playerInfo.Any()) return null;
            }
            else
            {
                playerInfo = playerInfoCache;
            }

            var allWhere = playerInfo.Where(p => p.L_Traded == false).ToList();            if (!allWhere.Any())
            {
                LogError.Write(new Exception("PlayerInfoServices"), "GetLeagueLeaders returned 0");
                return null; // TODO sghould also log
            }

            var playerInfoWhere = allWhere
                .OrderByDescending(z => z.I_Point)
                .ThenBy(y => y.I_Game)
                .Take(200).ToList();

            Mapper.CreateMap<PlayerInfo, PlayerInfoEntity>();
            var playerInfoEntities = Mapper.Map<List<PlayerInfo>, List<PlayerInfoEntity>>(playerInfoWhere);

            return playerInfoEntities;
        }

        public IEnumerable<PlayerInfoEntity> GetInjured()
        {
            var playerInfoCache = (List<PlayerInfo>)_caching.GetCachedItem("PlayerInfoGetAll");

            List<PlayerInfo> playerInfo = null; // This one ok as a list since it is not .Tolist() again
            if (playerInfoCache == null)
            {
                playerInfo = _unitOfWork.PlayerInfoRepository.GetAll().ToList();
                _caching.AddToCache("PlayerInfoGetAll", playerInfo);
                if (!playerInfo.Any()) return null;
            }
            else
            {
                playerInfo = playerInfoCache;
            }

            var playerInfoWhere = playerInfo.Where(p => p.L_IsInjured == true).ToList();            if (!playerInfoWhere.Any())
            {
                LogError.Write(new Exception("PlayerInfoServices"), "GetInjured returned 0");
                return null; // TODO sghould also log
            }

            Mapper.CreateMap<PlayerInfo, PlayerInfoEntity>();
            var playerInfoEntities = Mapper.Map<List<PlayerInfo>, List<PlayerInfoEntity>>(playerInfoWhere);

            return playerInfoEntities;
        }

        public IEnumerable<PlayerInfoEntity> GetAllWhere(string userEmail)
        {
            var playerInfoCache = (List<PlayerInfo>)_caching.GetCachedItem("PlayerInfoGetAll");

            List<PlayerInfo> playerInfo = null; // This one ok as a list since it is not .Tolist() again
            if (playerInfoCache == null)
            {
                playerInfo = _unitOfWork.PlayerInfoRepository.GetAll().ToList();
                _caching.AddToCache("PlayerInfoGetAll", playerInfo);
                if (!playerInfo.Any()) return null;
            }
            else
            {
                playerInfo = playerInfoCache;
            }

            var playerInfoWhere = playerInfo.Where(p => p.C_UserEmail == userEmail && p.L_Traded == false).ToList();            if (!playerInfoWhere.Any()) return null;

            Mapper.CreateMap<PlayerInfo, PlayerInfoEntity>();
            var playerInfoEntities = Mapper.Map<List<PlayerInfo>, List<PlayerInfoEntity>>(playerInfoWhere);

            return playerInfoEntities;
        }

        /// <summary>
        /// Crate PlayerInfo. should only be call/created from Nhl API
        /// </summary>
        /// <param name="playerInfoEntity"></param>
        /// <returns></returns>
        public bool Create(PlayerInfoEntity playerInfoEntity)
        {
            bool created;
            try
            {
                using (var scope = new TransactionScope())
                {
                    playerInfoEntity.C_Code = Guid.NewGuid().ToString();
                    Mapper.CreateMap<PlayerInfoEntity, PlayerInfo>();
                    var playerInfo = Mapper.Map<PlayerInfoEntity, PlayerInfo>(playerInfoEntity);

                    _unitOfWork.PlayerInfoRepository.Insert(playerInfo);
                    _unitOfWork.Save();
                    scope.Complete();
                    created = true;
                }
            }
            catch (TransactionAbortedException ex)
            {
                created = false;
                LogError.Write(ex, $"TransactionAbortedException in PlayerInfoServices.Create(PlayerInfoEntity playerInfoEntity) for id: {playerInfoEntity.C_Name} and code: {playerInfoEntity.C_Code}");

            }
            catch (ApplicationException ex)
            {
                created = false;
                LogError.Write(ex, $"ApplicationException in PlayerInfoServices.Create(PlayerInfoEntity playerInfoEntity) for id: {playerInfoEntity.C_Name} and code: {playerInfoEntity.C_Code}");
            }
            catch (Exception ex)
            {
                created = false;
                LogError.Write(ex, $"Exception in PlayerInfoServices.Create(PlayerInfoEntity playerInfoEntity) for id: {playerInfoEntity.C_Name} and code: {playerInfoEntity.C_Code}");
            }

            return created;
        }

        /// <summary>
        /// Crate list PlayerInfo. 
        /// </summary>
        /// <param name="playerInfoEntities"></param>
        /// <returns></returns>
        public bool CreateList(List<PlayerInfoEntity> playerInfoEntities)
        {
            bool created = false;
            try
            {
                foreach (var playerInfoEntity in playerInfoEntities)
                {
                    playerInfoEntity.C_Code = Guid.NewGuid().ToString();
                    Mapper.CreateMap<PlayerInfoEntity, PlayerInfo>();
                    var playerInfo = Mapper.Map<PlayerInfoEntity, PlayerInfo>(playerInfoEntity);

                    _unitOfWork.PlayerInfoRepository.Insert(playerInfo);
                }
                _unitOfWork.Save();
                created = true;
            }
            catch (Exception ex)
            {
                created = false;
                LogError.Write(ex, $"Exception in CreateList for team: {playerInfoEntities.ElementAt(0).C_Team}");
            }

            return created;
        }

        /// <summary>
        /// MAJOR: NEw api just add up stats under same record and  APIID
        /// Wherehas NHL.com would have same apiID but 2 records one on each team 
        /// </summary>
        /// <param name="playerInfoEntities"></param>
        /// <returns></returns>
        public bool Update(IEnumerable<PlayerInfoEntity> playerInfoEntities)
        {
            var updated = false;


            foreach (var playerInfoEntity in playerInfoEntities)
            {
                // via GEnRepo/UnitOFWork ctx
                var playerInfo = _unitOfWork.PlayerInfoRepository
                    .Get(x => x.I_ApiId == playerInfoEntity.I_ApiId);

                // player does not exist
                if (playerInfo == null)
                {
                    // create and continue: either new player from minor... or new season
                    Create(playerInfoEntity); // todo fix the data inserted
                    continue;
                }
                try
                {
                    #region old traded not in use anymore. Check by name/pos/team for unique. if not found then create
                    //    // #1 Check if player was traded... and only one in DB (meaning newly traded team player not in DB yet - !! 1st update instance since the trade occur!!)
                    //    // we compare with the only retrive yet in db
                    //    if (playerInfoEntity.C_Team != playerInfo.C_Team)
                    //    {
                    //        // make sure the newly traded team player not in DB yet
                    //        if (!ExistWithTrade(playerInfoEntity))
                    //        {
                    //            // get traded player and user details
                    //            playerInfoEntity.C_UserEmail = playerInfo.C_UserEmail;
                    //            playerInfoEntity.I_Round = playerInfo.I_Round;

                    //            playerInfoEntity.I_PtLastD =
                    //                _pastPlayerInfoServices.GetYesterdayWhere(playerInfoEntity.I_ApiId);
                    //            playerInfoEntity.I_PtLastW = _pastPlayerInfoServices.GetWeekWhere(playerInfoEntity.I_ApiId);
                    //            playerInfoEntity.I_PtLastM = _pastPlayerInfoServices.GetMonthWhere(playerInfoEntity.I_ApiId);

                    //            Create(playerInfoEntity);
                    //            //LogError.Write(new ApplicationException("Traded"),
                    //                //$"This player has been traded {playerInfoEntity.C_Name} to {playerInfoEntity.C_Team} ");

                    //            // update traded previous team player with new team in C_TradedTeam and FLAG has traded
                    //            playerInfo.C_TradedTeam = playerInfoEntity.C_Team;
                    //            playerInfo.L_Traded = true;
                    //            _unitOfWork.PlayerInfoRepository.Update(playerInfo);
                    //            _unitOfWork.Save(); 
                    //            continue;
                    //        }


                    //    }
                    //    // #2 Check if many player then get the one with new team. Then update his stats plus the one from traded player with old team
                    //    var playerInfoList =
                    //        _unitOfWork.PlayerInfoRepository.GetMany(p => p.I_ApiId == playerInfoEntity.I_ApiId).ToList();
                    //    if (playerInfoList.Count > 1)
                    //    {
                    //        var tradedPlayer =
                    //            playerInfoList.FirstOrDefault(x => x.L_Traded == true && x.C_TradedTeam != String.Empty);
                    //        if (tradedPlayer == null)
                    //        {
                    //            LogError.Write(new Exception("Issue in update stats traded"), playerInfoList.ToString());
                    //            continue;
                    //        }

                    //        // A) check if the playerInfoEntity passed in is the traded player exit this Update() if it is
                    //        if (tradedPlayer.C_Team != playerInfoEntity.C_Team)
                    //            continue;

                    //        // B) otherwise the playerInfo passed in is the new traded player then update his stats
                    //        var newTeamPlayer =
                    //            playerInfoList.FirstOrDefault(x => x.L_Traded == false && x.C_TradedTeam == String.Empty);
                    //        if (newTeamPlayer == null)
                    //        {
                    //            LogError.Write(new Exception("Issue in update stats traded"), playerInfoList.ToString());
                    //            continue;
                    //        }

                    //        // Meaning: inNewTeamPlayer.I_Game = (newUpdatedData.I_Game + ConstantNoFutureUpdateInDbTradedPlayer.I_Game);
                    //        // Basically cheating incorrectness of the api to store correct values


                    //        newTeamPlayer.I_Game = (playerInfoEntity.I_Game + tradedPlayer.I_Game);
                    //        newTeamPlayer.I_Goal = (playerInfoEntity.I_Goal + tradedPlayer.I_Goal);
                    //        newTeamPlayer.I_Assist = (playerInfoEntity.I_Assist + tradedPlayer.I_Assist);
                    //        newTeamPlayer.I_Point = (playerInfoEntity.I_Point + tradedPlayer.I_Point);
                    //        newTeamPlayer.C_Toi = playerInfoEntity.C_Toi;
                    //        newTeamPlayer.I_PpP = (playerInfoEntity.I_PpP + tradedPlayer.I_PpP);
                    //        newTeamPlayer.I_ShP = (playerInfoEntity.I_ShP + tradedPlayer.I_ShP);
                    //        newTeamPlayer.I_GwG = (playerInfoEntity.I_GwG + tradedPlayer.I_GwG);
                    //        newTeamPlayer.I_OtG = (playerInfoEntity.I_OtG + tradedPlayer.I_OtG);
                    //        newTeamPlayer.I_PtLastD = _pastPlayerInfoServices.GetYesterdayWhere(playerInfoEntity.I_ApiId
                    //            /*, newTeamPlayer.I_Point*/);
                    //        newTeamPlayer.I_PtLastW = _pastPlayerInfoServices.GetWeekWhere(playerInfoEntity.I_ApiId
                    //            /*, newTeamPlayer.I_Point*/);
                    //        newTeamPlayer.I_PtLastM = _pastPlayerInfoServices.GetMonthWhere(playerInfoEntity.I_ApiId
                    //            /*, newTeamPlayer.I_Point*/);
                    //        //newTeamPlayer.L_IsPlaying = _teamScheduleServices.IsTeamPlaying(playerInfoEntity.C_Team);
                    //        _unitOfWork.PlayerInfoRepository.Update(newTeamPlayer);//_bulkCtxEntities.Entry(newTeamPlayer).State = EntityState.Modified;//
                    //        _unitOfWork.Save();//_bulkCtxEntities.SaveChanges();//

                    //        //updated = true;
                    //        continue;
                    //    }

                    #endregion
                    // Necessary stats for from NHL API
                    playerInfo.I_Game = playerInfoEntity.I_Game;
                    playerInfo.I_Goal = playerInfoEntity.I_Goal;
                    playerInfo.I_Assist = playerInfoEntity.I_Assist;
                    playerInfo.I_Point = playerInfoEntity.I_Point;
                    playerInfo.C_Toi = playerInfoEntity.C_Toi;
                    playerInfo.I_PpP = playerInfoEntity.I_PpP;
                    playerInfo.I_ShP = playerInfoEntity.I_ShP;
                    playerInfo.L_IsRookie = playerInfoEntity.L_IsRookie; // so I can keep same data next year

                    playerInfo.I_PtLastD = _pastPlayerInfoServices.GetYesterdayWhere(playerInfoEntity);
                    playerInfo.I_PtLastW = _pastPlayerInfoServices.GetWeekWhere(playerInfoEntity);
                    playerInfo.I_PtLastM = _pastPlayerInfoServices.GetMonthWhere(playerInfoEntity);
                    //playerInfo.L_IsPlaying = _teamScheduleServices.IsTeamPlaying(playerInfoEntity.C_Team);

                    _unitOfWork.PlayerInfoRepository.Update(playerInfo);


                }
                catch (ApplicationException ex)
                {
                    LogError.Write(ex,
                        $"ApplicationException in PlayerInfoServices.Create(IEnumerable<PlayerInfoEntity> playerInfoEntities)() for id: {playerInfo.C_Name} and code: {playerInfo.C_Code}");
                }
                catch (Exception ex)
                {
                    LogError.Write(ex,
                        $"Exception in PlayerInfoServices.Create(IEnumerable<PlayerInfoEntity> playerInfoEntities)() for id: {playerInfo.C_Name} and code: {playerInfo.C_Code}");
                }
            }

            try
            {
                // Save the remainder - no its the team
                _unitOfWork.Save();
                _unitOfWork = new UnitOfWork();
                updated = true;
            }
            catch (Exception ex)
            {
                LogError.Write(ex,
                        "Exception in PlayerInfoServices.Update(IEnumerable<PlayerInfoEntity> playerInfoEntities)()  on Save ALL to Context");
                updated = false;
                return updated;
            }

            return updated;
        }

        
        public bool UpdateFromMySportsFeeds(IEnumerable<PlayerInfoEntity> playerInfoEntities)
        {
            var updated = false;


            foreach (var playerInfoEntity in playerInfoEntities)
            {
                // via GEnRepo/UnitOFWork ctx
                var playerInfo = _unitOfWork.PlayerInfoRepository
                    .Get(x => x.C_Name == playerInfoEntity.C_Name && x.C_Team == playerInfoEntity.C_Team && x.C_Pos == playerInfoEntity.C_Pos);

                // player does not exist
                if (playerInfo == null)
                {
                    // create and continue
                    // TODO - Will have to find way to track exchange...Same concept. ID stays the same, but stats are split on the different team player stats page
                    // TODO - will have to do a web scrap to load CBC sport ID. Then if null check for id, if exists add points togheter see section old traded!
                    playerInfoEntity.C_UserEmail = "";
                    playerInfoEntity.C_TradedTeam = "";
                    Create(playerInfoEntity); // todo fix the data inserted
                    continue;
                }
                try
                {
                    #region old traded not in use anymore. Check by name/pos/team for unique. if not found then create
                    //    // #1 Check if player was traded... and only one in DB (meaning newly traded team player not in DB yet - !! 1st update instance since the trade occur!!)
                    //    // we compare with the only retrive yet in db
                    //    if (playerInfoEntity.C_Team != playerInfo.C_Team)
                    //    {
                    //        // make sure the newly traded team player not in DB yet
                    //        if (!ExistWithTrade(playerInfoEntity))
                    //        {
                    //            // get traded player and user details
                    //            playerInfoEntity.C_UserEmail = playerInfo.C_UserEmail;
                    //            playerInfoEntity.I_Round = playerInfo.I_Round;

                    //            playerInfoEntity.I_PtLastD =
                    //                _pastPlayerInfoServices.GetYesterdayWhere(playerInfoEntity.I_ApiId);
                    //            playerInfoEntity.I_PtLastW = _pastPlayerInfoServices.GetWeekWhere(playerInfoEntity.I_ApiId);
                    //            playerInfoEntity.I_PtLastM = _pastPlayerInfoServices.GetMonthWhere(playerInfoEntity.I_ApiId);

                    //            Create(playerInfoEntity);
                    //            //LogError.Write(new ApplicationException("Traded"),
                    //                //$"This player has been traded {playerInfoEntity.C_Name} to {playerInfoEntity.C_Team} ");

                    //            // update traded previous team player with new team in C_TradedTeam and FLAG has traded
                    //            playerInfo.C_TradedTeam = playerInfoEntity.C_Team;
                    //            playerInfo.L_Traded = true;
                    //            _unitOfWork.PlayerInfoRepository.Update(playerInfo);
                    //            _unitOfWork.Save(); 
                    //            continue;
                    //        }


                    //    }
                    //    // #2 Check if many player then get the one with new team. Then update his stats plus the one from traded player with old team
                    //    var playerInfoList =
                    //        _unitOfWork.PlayerInfoRepository.GetMany(p => p.I_ApiId == playerInfoEntity.I_ApiId).ToList();
                    //    if (playerInfoList.Count > 1)
                    //    {
                    //        var tradedPlayer =
                    //            playerInfoList.FirstOrDefault(x => x.L_Traded == true && x.C_TradedTeam != String.Empty);
                    //        if (tradedPlayer == null)
                    //        {
                    //            LogError.Write(new Exception("Issue in update stats traded"), playerInfoList.ToString());
                    //            continue;
                    //        }

                    //        // A) check if the playerInfoEntity passed in is the traded player exit this Update() if it is
                    //        if (tradedPlayer.C_Team != playerInfoEntity.C_Team)
                    //            continue;

                    //        // B) otherwise the playerInfo passed in is the new traded player then update his stats
                    //        var newTeamPlayer =
                    //            playerInfoList.FirstOrDefault(x => x.L_Traded == false && x.C_TradedTeam == String.Empty);
                    //        if (newTeamPlayer == null)
                    //        {
                    //            LogError.Write(new Exception("Issue in update stats traded"), playerInfoList.ToString());
                    //            continue;
                    //        }

                    //        // Meaning: inNewTeamPlayer.I_Game = (newUpdatedData.I_Game + ConstantNoFutureUpdateInDbTradedPlayer.I_Game);
                    //        // Basically cheating incorrectness of the api to store correct values


                    //        newTeamPlayer.I_Game = (playerInfoEntity.I_Game + tradedPlayer.I_Game);
                    //        newTeamPlayer.I_Goal = (playerInfoEntity.I_Goal + tradedPlayer.I_Goal);
                    //        newTeamPlayer.I_Assist = (playerInfoEntity.I_Assist + tradedPlayer.I_Assist);
                    //        newTeamPlayer.I_Point = (playerInfoEntity.I_Point + tradedPlayer.I_Point);
                    //        newTeamPlayer.C_Toi = playerInfoEntity.C_Toi;
                    //        newTeamPlayer.I_PpP = (playerInfoEntity.I_PpP + tradedPlayer.I_PpP);
                    //        newTeamPlayer.I_ShP = (playerInfoEntity.I_ShP + tradedPlayer.I_ShP);
                    //        newTeamPlayer.I_GwG = (playerInfoEntity.I_GwG + tradedPlayer.I_GwG);
                    //        newTeamPlayer.I_OtG = (playerInfoEntity.I_OtG + tradedPlayer.I_OtG);
                    //        newTeamPlayer.I_PtLastD = _pastPlayerInfoServices.GetYesterdayWhere(playerInfoEntity.I_ApiId
                    //            /*, newTeamPlayer.I_Point*/);
                    //        newTeamPlayer.I_PtLastW = _pastPlayerInfoServices.GetWeekWhere(playerInfoEntity.I_ApiId
                    //            /*, newTeamPlayer.I_Point*/);
                    //        newTeamPlayer.I_PtLastM = _pastPlayerInfoServices.GetMonthWhere(playerInfoEntity.I_ApiId
                    //            /*, newTeamPlayer.I_Point*/);
                    //        //newTeamPlayer.L_IsPlaying = _teamScheduleServices.IsTeamPlaying(playerInfoEntity.C_Team);
                    //        _unitOfWork.PlayerInfoRepository.Update(newTeamPlayer);//_bulkCtxEntities.Entry(newTeamPlayer).State = EntityState.Modified;//
                    //        _unitOfWork.Save();//_bulkCtxEntities.SaveChanges();//

                    //        //updated = true;
                    //        continue;
                    //    }

                    #endregion
                    // Necessary stats for from NHL API
                    playerInfo.I_Game = playerInfoEntity.I_Game;
                    playerInfo.I_Goal = playerInfoEntity.I_Goal;
                    playerInfo.I_Assist = playerInfoEntity.I_Assist;
                    playerInfo.I_Point = playerInfoEntity.I_Point;
                    playerInfo.C_Toi = playerInfoEntity.C_Toi;
                    playerInfo.I_PpP = playerInfoEntity.I_PpP;
                    playerInfo.I_ShP = playerInfoEntity.I_ShP;

                    // removed with Scarpping
                    //playerInfo.I_GwG = playerInfoEntity.I_GwG;
                    //playerInfo.I_OtG = playerInfoEntity.I_OtG;

                    playerInfo.I_PtLastD = _pastPlayerInfoServices.GetYesterdayWhere(playerInfoEntity);
                    playerInfo.I_PtLastW = _pastPlayerInfoServices.GetWeekWhere(playerInfoEntity);
                    playerInfo.I_PtLastM = _pastPlayerInfoServices.GetMonthWhere(playerInfoEntity);
                    //playerInfo.L_IsPlaying = _teamScheduleServices.IsTeamPlaying(playerInfoEntity.C_Team);

                    _unitOfWork.PlayerInfoRepository.Update(playerInfo);


                }
                catch (ApplicationException ex)
                {
                    LogError.Write(ex,
                        $"ApplicationException in PlayerInfoServices.Create(IEnumerable<PlayerInfoEntity> playerInfoEntities)() for id: {playerInfo.C_Name} and code: {playerInfo.C_Code}");
                }
                catch (Exception ex)
                {
                    LogError.Write(ex,
                        $"Exception in PlayerInfoServices.Create(IEnumerable<PlayerInfoEntity> playerInfoEntities)() for id: {playerInfo.C_Name} and code: {playerInfo.C_Code}");
                }
            }

            try
            {
                // Save the remainder - no its the team
                _unitOfWork.Save();
                _unitOfWork = new UnitOfWork();
                updated = true;
            }
            catch (Exception ex)
            {
                LogError.Write(ex,
                        "Exception in PlayerInfoServices.Update(IEnumerable<PlayerInfoEntity> playerInfoEntities)()  on Save ALL to Context");
                updated = false;
                return updated;
            }

            return updated;
        }

        public bool UpdateAvg()
        {
            var players = _unitOfWork.PlayerInfoRepository.GetMany(p => p.I_Point > 0).ToList();
            if (!players.Any())
            {
                LogError.Write(new Exception("No players found form DB.."), "Exception in UpdateAvg ");
                return false;
            }
            foreach (var playerInfo in players)
            {
                try
                {
                    var game = playerInfo.I_Game;
                    var point = (double)playerInfo.I_Point;
                    var avg = point / game;
                    playerInfo.F_Avg = Math.Round(avg, 2);
                    _unitOfWork.PlayerInfoRepository.Update(playerInfo);
                    _unitOfWork.Save();
                }
                catch (Exception ex)
                {
                    LogError.Write(ex, "Exception in UpdateAvg at player code: " + playerInfo.C_Code);
                }
            }

            return true;
        }


        // @@@@ Just use same query to exec all 3 uptate
        public bool UpdateStatus()
        {
            _unitOfWork.ClearAllStatus();
            var players = _unitOfWork.PlayerInfoRepository.GetMany(p => p.C_UserEmail != string.Empty).ToList();
            if (!players.Any())
            {
                LogError.Write(new Exception("No players found form DB.."), "Exception in Update status ");
                return false;
            }

            var users = players.Select(x => x.C_UserEmail).Distinct().ToList();

            foreach (var user in users)
            {

                //Statuses.Out = 3. Get 2 worst by user
                try
                {
                    var worst2Players =
                        players.Where(p => p.C_UserEmail == user && p.L_Traded == false)
                            .OrderBy(p => p.I_Point) // less to most
                            .ThenByDescending(p => p.I_Game) // most to less
                            .Take(2).ToList();
                    var playerOut1 = worst2Players.ElementAt(0);
                    playerOut1.I_Status = (int)Statuses.Out;
                    var playerOut2 = worst2Players.ElementAt(1);
                    playerOut2.I_Status = (int)Statuses.Out;
                    _unitOfWork.PlayerInfoRepository.Update(playerOut1);
                    _unitOfWork.PlayerInfoRepository.Update(playerOut2);
                    _unitOfWork.Save();
                }
                catch (Exception ex)
                {
                    LogError.Write(ex, "Exception in UpdateStatus at user: " + user);
                }
            }

            //Statuses.Top = 1, check if for a given user if one of his picks is top of round
            for (int i = 1; i <= Constants.Rules.Rounds; i++)
            {
                try
                {
                    var player = players.Where(p => p.I_Round == i && p.L_Traded == false)
                        .OrderByDescending(p => p.I_Point) // most point
                        .ThenBy(p => p.I_Game) // with less game
                        .Take(1).ToList();
                    var topOfRound = player.ElementAt(0);
                    topOfRound.I_Status = (int)Statuses.Top;
                    _unitOfWork.PlayerInfoRepository.Update(topOfRound);
                    _unitOfWork.Save();
                }
                catch (Exception ex)
                {
                    LogError.Write(ex, "Exception in UpdateStatus at top round: " + i);
                }
            }

            //Statuses.Worst = 2, check if for a given user if one of his picks is worst of round
            for (int i = 1; i <= Constants.Rules.Rounds; i++)
            {
                try
                {
                    var player = players.Where(p => p.I_Round == i && p.L_Traded == false && p.I_Status != (int)Statuses.Out)
                        .OrderBy(p => p.I_Point) // less point
                        .ThenByDescending(p => p.I_Game) // with most game
                        .Take(1).ToList();
                    var worstOfRound = player.ElementAt(0);
                    worstOfRound.I_Status = (int)Statuses.Worst;
                    _unitOfWork.PlayerInfoRepository.Update(worstOfRound);
                    _unitOfWork.Save();
                }
                catch (Exception ex)
                {
                    LogError.Write(ex, "Exception in UpdateStatus at wost round: " + i);
                }
            }


            return true;
        }

        public bool UpdateInjuryStatus()
        {
            var updated = false;

            _unitOfWork.ClearAllInjuryStatus();
            var injuryList = InjuredApiTransactions.GetInjuredList().ToList();
            var playerInfoEntities = _unitOfWork.PlayerInfoRepository.GetAll().ToList();

            foreach (var injured in injuryList)
            {
                try
                {
                    var playerInfo = playerInfoEntities.FirstOrDefault(p => p.C_Name == injured.Name);// new api: change compare name only since using full name... removed: && p.C_Pos == injured.Position);
                    if (playerInfo == null) continue;

                    playerInfo.L_IsInjured = true;
                    playerInfo.C_InjStatus = injured.Status;
                    playerInfo.C_InjDetails = injured.Details;
                    _unitOfWork.PlayerInfoRepository.Update(playerInfo);
                    _unitOfWork.Save();

                }
                catch (Exception e)
                {
                    LogError.Write(e, "Exception in UpdateInjuryStatus() at injured: " + injured.Name);
                }
                updated = true;
            }
            return updated;
        }

        public bool Delete(string playerInfoCode)
        {
            throw new NotImplementedException();
        }

        public bool Exist(PlayerInfoEntity playerInfoEntity)
        {
            var playerExist = _unitOfWork.PlayerInfoRepository.Get(x => x.C_Name == playerInfoEntity.C_Name && x.C_Team == playerInfoEntity.C_Team && x.C_Pos == playerInfoEntity.C_Pos);
            return playerExist != null;
        }

        ///// <summary>
        ///// Retrun true if many apiId already in dB
        ///// </summary>
        ///// <param name="playerInfoEntity"></param>
        ///// <returns></returns>
        //public bool ExistWithTrade(PlayerInfoEntity playerInfoEntity)
        //{
        //    var playerExists = _unitOfWork.PlayerInfoRepository.GetMany(p => p.I_ApiId == playerInfoEntity.I_ApiId);
        //    if (playerExists.Count() > 1)
        //        return true;
        //    return false;
        //}
    }
}
