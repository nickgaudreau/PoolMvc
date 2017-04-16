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
    
    public class PlayoffPlayerInfoServices : IPlayoffPlayerInfoServices
    {
        private IUnitOfWork _unitOfWork; // not readonly to be able to dispose and create new context during split bulk save changes
        private readonly IPastPlayerInfoServices _pastPlayerInfoServices;
        //private readonly ITeamScheduleServices _teamScheduleServices;
        private readonly ICaching _caching;

        ///// <summary>
        ///// For internal use
        ///// </summary>
        //internal PlayerInfoServices() { }
        
        public PlayoffPlayerInfoServices(UnitOfWork unitOfWork, IPastPlayerInfoServices pastPlayerInfoServices)
        {
            _unitOfWork = unitOfWork;//new UnitOfWork();
            _pastPlayerInfoServices = pastPlayerInfoServices;//new PastPlayerInfoServices();
            //_teamScheduleServices = new TeamScheduleServices();
            _caching = new Caching();
        }

        /// <summary>
        /// Fetches PlayoffPlayerInfo details by code
        /// </summary>
        /// <param name="playoffPlayerInfoCode"></param>
        /// <returns></returns>
        public PlayoffPlayerInfoEntity GetById(string playoffPlayerInfoCode)
        {
            var playoffPlayerInfo = _unitOfWork.PlayoffPlayerInfoRepository.GetByID(playoffPlayerInfoCode);            if (playoffPlayerInfo == null) return null;

            Mapper.CreateMap<PlayoffPlayerInfo, PlayoffPlayerInfoEntity>();
            var playoffPlayerInfoEntity = Mapper.Map<PlayoffPlayerInfo, PlayoffPlayerInfoEntity>(playoffPlayerInfo);

            return playoffPlayerInfoEntity;
        }

        public IEnumerable<PlayoffPlayerInfoEntity> GetBestPerRound(int round)
        {
            var playoffPlayerInfoCache = (List<PlayoffPlayerInfo>)_caching.GetCachedItem("PlayoffPlayerInfoGetAll");

            List<PlayoffPlayerInfo> playoffPlayerInfo = null; // This one ok as a list since it is not .Tolist() again
            if (playoffPlayerInfoCache == null)
            {
                playoffPlayerInfo = _unitOfWork.PlayoffPlayerInfoRepository.GetAll().ToList();
                _caching.AddToCache("PlayoffPlayerInfoGetAll", playoffPlayerInfo);
                if (!playoffPlayerInfo.Any()) return null;
            }
            else
            {
                playoffPlayerInfo = playoffPlayerInfoCache;
            }

            var playoffPlayerInfoOrderedByRound = playoffPlayerInfo
                .Where(x => x.I_Round == round && x.L_Traded == false && x.C_UserEmail != String.Empty)
                .OrderByDescending(p => p.I_Point)
                .ThenBy(g => g.I_Game).ToList();

            Mapper.CreateMap<PlayoffPlayerInfo, PlayoffPlayerInfoEntity>();
            var playoffPlayerInfoEntities = Mapper.Map<List<PlayoffPlayerInfo>, List<PlayoffPlayerInfoEntity>>(playoffPlayerInfoOrderedByRound);

            return playoffPlayerInfoEntities;
        }

        /// <summary>
        /// Fetches all PlayoffPlayerInfo
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PlayoffPlayerInfoEntity> GetAll()
        {
            var playoffPlayerInfos = _unitOfWork.PlayoffPlayerInfoRepository.GetAll().ToList();            if (!playoffPlayerInfos.Any()) return null; // TODO sghould also log

            Mapper.CreateMap<PlayoffPlayerInfo, PlayoffPlayerInfoEntity>();
            var playoffPlayerInfoEntities = Mapper.Map<List<PlayoffPlayerInfo>, List<PlayoffPlayerInfoEntity>>(playoffPlayerInfos);

            return playoffPlayerInfoEntities;
        }

        public IEnumerable<PlayoffPlayerInfoEntity> GetUndrafted()
        {
            var playoffPlayerInfoCache = (List<PlayoffPlayerInfo>)_caching.GetCachedItem("PlayoffPlayerInfoGetAll");

            List<PlayoffPlayerInfo> playoffPlayerInfo = null; // This one ok as a list since it is not .Tolist() again
            if (playoffPlayerInfoCache == null)
            {
                playoffPlayerInfo = _unitOfWork.PlayoffPlayerInfoRepository.GetAll().ToList();
                _caching.AddToCache("PlayoffPlayerInfoGetAll", playoffPlayerInfo);
                if (!playoffPlayerInfo.Any()) return null;
            }
            else
            {
                playoffPlayerInfo = playoffPlayerInfoCache;
            }

            var allWhere = playoffPlayerInfo.Where(p => p.C_UserEmail == String.Empty && p.L_Traded == false).ToList();            if (!allWhere.Any())
            {
                LogError.Write(new Exception("PlayerInfoServices"), "GetUndrafted returned 0");
                return null; // TODO sghould also log
            }

            var playoffPlayerInfoWhere = allWhere
                .OrderByDescending(z => z.I_Point)
                .ThenBy(y => y.I_Game)
                .Take(50).ToList();

            Mapper.CreateMap<PlayoffPlayerInfo, PlayoffPlayerInfoEntity>();
            var playoffPlayerInfoEntities = Mapper.Map<List<PlayoffPlayerInfo>, List<PlayoffPlayerInfoEntity>>(playoffPlayerInfoWhere);

            return playoffPlayerInfoEntities;
        }

        public IEnumerable<PlayoffPlayerInfoEntity> GetLeagueLeaders()
        {
            var playoffPlayerInfoCache = (List<PlayoffPlayerInfo>)_caching.GetCachedItem("PlayoffPlayerInfoGetAll");

            List<PlayoffPlayerInfo> playoffPlayerInfo = null; // This one ok as a list since it is not .Tolist() again
            if (playoffPlayerInfoCache == null)
            {
                playoffPlayerInfo = _unitOfWork.PlayoffPlayerInfoRepository.GetAll().ToList();
                _caching.AddToCache("PlayoffPlayerInfoGetAll", playoffPlayerInfo);
                if (!playoffPlayerInfo.Any()) return null;
            }
            else
            {
                playoffPlayerInfo = playoffPlayerInfoCache;
            }

            var allWhere = playoffPlayerInfo.Where(p => p.L_Traded == false).ToList();            if (!allWhere.Any())
            {
                LogError.Write(new Exception("PlayerInfoServices"), "GetLeagueLeaders returned 0");
                return null; // TODO sghould also log
            }

            var playoffPlayerInfoWhere = allWhere
                .OrderByDescending(z => z.I_Point)
                .ThenBy(y => y.I_Game)
                .Take(200).ToList();

            Mapper.CreateMap<PlayoffPlayerInfo, PlayoffPlayerInfoEntity>();
            var playoffPlayerInfoEntities = Mapper.Map<List<PlayoffPlayerInfo>, List<PlayoffPlayerInfoEntity>>(playoffPlayerInfoWhere);

            return playoffPlayerInfoEntities;
        }

        public IEnumerable<PlayoffPlayerInfoEntity> GetInjured()
        {
            var playoffPlayerInfoCache = (List<PlayoffPlayerInfo>)_caching.GetCachedItem("PlayoffPlayerInfoGetAll");

            List<PlayoffPlayerInfo> playoffPlayerInfo = null; // This one ok as a list since it is not .Tolist() again
            if (playoffPlayerInfoCache == null)
            {
                playoffPlayerInfo = _unitOfWork.PlayoffPlayerInfoRepository.GetAll().ToList();
                _caching.AddToCache("PlayoffPlayerInfoGetAll", playoffPlayerInfo);
                if (!playoffPlayerInfo.Any()) return null;
            }
            else
            {
                playoffPlayerInfo = playoffPlayerInfoCache;
            }

            var playoffPlayerInfoWhere = playoffPlayerInfo.Where(p => p.L_IsInjured == true).ToList();            if (!playoffPlayerInfoWhere.Any())
            {
                LogError.Write(new Exception("PlayerInfoServices"), "GetInjured returned 0");
                return null; // TODO sghould also log
            }

            Mapper.CreateMap<PlayoffPlayerInfo, PlayoffPlayerInfoEntity>();
            var playoffPlayerInfoEntities = Mapper.Map<List<PlayoffPlayerInfo>, List<PlayoffPlayerInfoEntity>>(playoffPlayerInfoWhere);

            return playoffPlayerInfoEntities;
        }

        public IEnumerable<PlayoffPlayerInfoEntity> GetAllWhere(string userEmail)
        {
            var playoffPlayerInfoCache = (List<PlayoffPlayerInfo>)_caching.GetCachedItem("PlayoffPlayerInfoGetAll");

            List<PlayoffPlayerInfo> playoffPlayerInfo = null; // This one ok as a list since it is not .Tolist() again
            if (playoffPlayerInfoCache == null)
            {
                playoffPlayerInfo = _unitOfWork.PlayoffPlayerInfoRepository.GetAll().ToList();
                _caching.AddToCache("PlayoffPlayerInfoGetAll", playoffPlayerInfo);
                if (!playoffPlayerInfo.Any()) return null;
            }
            else
            {
                playoffPlayerInfo = playoffPlayerInfoCache;
            }

            var playoffPlayerInfoWhere = playoffPlayerInfo.Where(p => p.C_UserEmail == userEmail && p.L_Traded == false).ToList();            if (!playoffPlayerInfoWhere.Any()) return null;

            Mapper.CreateMap<PlayoffPlayerInfo, PlayoffPlayerInfoEntity>();
            var playoffPlayerInfoEntities = Mapper.Map<List<PlayoffPlayerInfo>, List<PlayoffPlayerInfoEntity>>(playoffPlayerInfoWhere);

            return playoffPlayerInfoEntities;
        }

        /// <summary>
        /// Crate PlayoffPlayerInfo. should only be call/created from Nhl API
        /// </summary>
        /// <param name="playoffPlayerInfoEntity"></param>
        /// <returns></returns>
        public bool Create(PlayoffPlayerInfoEntity playoffPlayerInfoEntity)
        {
            bool created;
            try
            {
                using (var scope = new TransactionScope())
                {
                    playoffPlayerInfoEntity.C_Code = Guid.NewGuid().ToString();
                    Mapper.CreateMap<PlayoffPlayerInfoEntity, PlayoffPlayerInfo>();
                    var playoffPlayerInfo = Mapper.Map<PlayoffPlayerInfoEntity, PlayoffPlayerInfo>(playoffPlayerInfoEntity);

                    _unitOfWork.PlayoffPlayerInfoRepository.Insert(playoffPlayerInfo);
                    _unitOfWork.Save();
                    scope.Complete();
                    created = true;
                }
            }
            catch (TransactionAbortedException ex)
            {
                created = false;
                LogError.Write(ex, $"TransactionAbortedException in PlayerInfoServices.Create(PlayoffPlayerInfoEntity playoffPlayerInfoEntity) for id: {playoffPlayerInfoEntity.C_Name} and code: {playoffPlayerInfoEntity.C_Code}");

            }
            catch (ApplicationException ex)
            {
                created = false;
                LogError.Write(ex, $"ApplicationException in PlayerInfoServices.Create(PlayoffPlayerInfoEntity playoffPlayerInfoEntity) for id: {playoffPlayerInfoEntity.C_Name} and code: {playoffPlayerInfoEntity.C_Code}");
            }
            catch (Exception ex)
            {
                created = false;
                LogError.Write(ex, $"Exception in PlayerInfoServices.Create(PlayoffPlayerInfoEntity playoffPlayerInfoEntity) for id: {playoffPlayerInfoEntity.C_Name} and code: {playoffPlayerInfoEntity.C_Code}");
            }

            return created;
        }

        /// <summary>
        /// Crate list PlayoffPlayerInfo. 
        /// </summary>
        /// <param name="playoffPlayerInfoEntities"></param>
        /// <returns></returns>
        public bool CreateList(List<PlayoffPlayerInfoEntity> playoffPlayerInfoEntities)
        {
            bool created = false;
            try
            {
                foreach (var playoffPlayerInfoEntity in playoffPlayerInfoEntities)
                {
                    playoffPlayerInfoEntity.C_Code = Guid.NewGuid().ToString();
                    Mapper.CreateMap<PlayoffPlayerInfoEntity, PlayoffPlayerInfo>();
                    var playoffPlayerInfo = Mapper.Map<PlayoffPlayerInfoEntity, PlayoffPlayerInfo>(playoffPlayerInfoEntity);

                    _unitOfWork.PlayoffPlayerInfoRepository.Insert(playoffPlayerInfo);
                }
                _unitOfWork.Save();
                created = true;
            }
            catch (Exception ex)
            {
                created = false;
                LogError.Write(ex, $"Exception in CreateList for team: {playoffPlayerInfoEntities.ElementAt(0).C_Team}");
            }

            return created;
        }

        /// <summary>
        /// MAJOR: NEw api just add up stats under same record and  APIID
        /// Wherehas NHL.com would have same apiID but 2 records one on each team 
        /// </summary>
        /// <param name="playoffPlayerInfoEntities"></param>
        /// <returns></returns>
        public bool Update(IEnumerable<PlayoffPlayerInfoEntity> playoffPlayerInfoEntities)
        {
            var updated = false;


            foreach (var playoffPlayerInfoEntity in playoffPlayerInfoEntities)
            {
                // via GEnRepo/UnitOFWork ctx
                var playoffPlayerInfo = _unitOfWork.PlayoffPlayerInfoRepository
                    .Get(x => x.I_ApiId == playoffPlayerInfoEntity.I_ApiId);

                // player does not exist
                if (playoffPlayerInfo == null)
                {
                    // create and continue: either new player from minor... or new season
                    Create(playoffPlayerInfoEntity); // todo fix the data inserted
                    continue;
                }
                try
                {
                    #region old traded not in use anymore. Check by name/pos/team for unique. if not found then create
                    //    // #1 Check if player was traded... and only one in DB (meaning newly traded team player not in DB yet - !! 1st update instance since the trade occur!!)
                    //    // we compare with the only retrive yet in db
                    //    if (playoffPlayerInfoEntity.C_Team != playoffPlayerInfo.C_Team)
                    //    {
                    //        // make sure the newly traded team player not in DB yet
                    //        if (!ExistWithTrade(playoffPlayerInfoEntity))
                    //        {
                    //            // get traded player and user details
                    //            playoffPlayerInfoEntity.C_UserEmail = playoffPlayerInfo.C_UserEmail;
                    //            playoffPlayerInfoEntity.I_Round = playoffPlayerInfo.I_Round;

                    //            playoffPlayerInfoEntity.I_PtLastD =
                    //                _pastPlayerInfoServices.GetYesterdayWhere(playoffPlayerInfoEntity.I_ApiId);
                    //            playoffPlayerInfoEntity.I_PtLastW = _pastPlayerInfoServices.GetWeekWhere(playoffPlayerInfoEntity.I_ApiId);
                    //            playoffPlayerInfoEntity.I_PtLastM = _pastPlayerInfoServices.GetMonthWhere(playoffPlayerInfoEntity.I_ApiId);

                    //            Create(playoffPlayerInfoEntity);
                    //            //LogError.Write(new ApplicationException("Traded"),
                    //                //$"This player has been traded {playoffPlayerInfoEntity.C_Name} to {playoffPlayerInfoEntity.C_Team} ");

                    //            // update traded previous team player with new team in C_TradedTeam and FLAG has traded
                    //            playoffPlayerInfo.C_TradedTeam = playoffPlayerInfoEntity.C_Team;
                    //            playoffPlayerInfo.L_Traded = true;
                    //            _unitOfWork.PlayoffPlayerInfoRepository.Update(playoffPlayerInfo);
                    //            _unitOfWork.Save(); 
                    //            continue;
                    //        }


                    //    }
                    //    // #2 Check if many player then get the one with new team. Then update his stats plus the one from traded player with old team
                    //    var playoffPlayerInfoList =
                    //        _unitOfWork.PlayoffPlayerInfoRepository.GetMany(p => p.I_ApiId == playoffPlayerInfoEntity.I_ApiId).ToList();
                    //    if (playoffPlayerInfoList.Count > 1)
                    //    {
                    //        var tradedPlayer =
                    //            playoffPlayerInfoList.FirstOrDefault(x => x.L_Traded == true && x.C_TradedTeam != String.Empty);
                    //        if (tradedPlayer == null)
                    //        {
                    //            LogError.Write(new Exception("Issue in update stats traded"), playoffPlayerInfoList.ToString());
                    //            continue;
                    //        }

                    //        // A) check if the playoffPlayerInfoEntity passed in is the traded player exit this Update() if it is
                    //        if (tradedPlayer.C_Team != playoffPlayerInfoEntity.C_Team)
                    //            continue;

                    //        // B) otherwise the playoffPlayerInfo passed in is the new traded player then update his stats
                    //        var newTeamPlayer =
                    //            playoffPlayerInfoList.FirstOrDefault(x => x.L_Traded == false && x.C_TradedTeam == String.Empty);
                    //        if (newTeamPlayer == null)
                    //        {
                    //            LogError.Write(new Exception("Issue in update stats traded"), playoffPlayerInfoList.ToString());
                    //            continue;
                    //        }

                    //        // Meaning: inNewTeamPlayer.I_Game = (newUpdatedData.I_Game + ConstantNoFutureUpdateInDbTradedPlayer.I_Game);
                    //        // Basically cheating incorrectness of the api to store correct values


                    //        newTeamPlayer.I_Game = (playoffPlayerInfoEntity.I_Game + tradedPlayer.I_Game);
                    //        newTeamPlayer.I_Goal = (playoffPlayerInfoEntity.I_Goal + tradedPlayer.I_Goal);
                    //        newTeamPlayer.I_Assist = (playoffPlayerInfoEntity.I_Assist + tradedPlayer.I_Assist);
                    //        newTeamPlayer.I_Point = (playoffPlayerInfoEntity.I_Point + tradedPlayer.I_Point);
                    //        newTeamPlayer.C_Toi = playoffPlayerInfoEntity.C_Toi;
                    //        newTeamPlayer.I_PpP = (playoffPlayerInfoEntity.I_PpP + tradedPlayer.I_PpP);
                    //        newTeamPlayer.I_ShP = (playoffPlayerInfoEntity.I_ShP + tradedPlayer.I_ShP);
                    //        newTeamPlayer.I_GwG = (playoffPlayerInfoEntity.I_GwG + tradedPlayer.I_GwG);
                    //        newTeamPlayer.I_OtG = (playoffPlayerInfoEntity.I_OtG + tradedPlayer.I_OtG);
                    //        newTeamPlayer.I_PtLastD = _pastPlayerInfoServices.GetYesterdayWhere(playoffPlayerInfoEntity.I_ApiId
                    //            /*, newTeamPlayer.I_Point*/);
                    //        newTeamPlayer.I_PtLastW = _pastPlayerInfoServices.GetWeekWhere(playoffPlayerInfoEntity.I_ApiId
                    //            /*, newTeamPlayer.I_Point*/);
                    //        newTeamPlayer.I_PtLastM = _pastPlayerInfoServices.GetMonthWhere(playoffPlayerInfoEntity.I_ApiId
                    //            /*, newTeamPlayer.I_Point*/);
                    //        //newTeamPlayer.L_IsPlaying = _teamScheduleServices.IsTeamPlaying(playoffPlayerInfoEntity.C_Team);
                    //        _unitOfWork.PlayoffPlayerInfoRepository.Update(newTeamPlayer);//_bulkCtxEntities.Entry(newTeamPlayer).State = EntityState.Modified;//
                    //        _unitOfWork.Save();//_bulkCtxEntities.SaveChanges();//

                    //        //updated = true;
                    //        continue;
                    //    }

                    #endregion
                    // Necessary stats for from NHL API
                    playoffPlayerInfo.I_Game = playoffPlayerInfoEntity.I_Game;
                    playoffPlayerInfo.I_Goal = playoffPlayerInfoEntity.I_Goal;
                    playoffPlayerInfo.I_Assist = playoffPlayerInfoEntity.I_Assist;
                    playoffPlayerInfo.I_Point = playoffPlayerInfoEntity.I_Point;
                    playoffPlayerInfo.C_Toi = playoffPlayerInfoEntity.C_Toi;
                    playoffPlayerInfo.I_PpP = playoffPlayerInfoEntity.I_PpP;
                    playoffPlayerInfo.I_ShP = playoffPlayerInfoEntity.I_ShP;
                    playoffPlayerInfo.L_IsRookie = playoffPlayerInfoEntity.L_IsRookie; // so I can keep same data next year

                    playoffPlayerInfo.I_PtLastD = _pastPlayerInfoServices.GetYesterdayWhere(playoffPlayerInfoEntity);
                    playoffPlayerInfo.I_PtLastW = _pastPlayerInfoServices.GetWeekWhere(playoffPlayerInfoEntity);
                    playoffPlayerInfo.I_PtLastM = _pastPlayerInfoServices.GetMonthWhere(playoffPlayerInfoEntity);
                    //playoffPlayerInfo.L_IsPlaying = _teamScheduleServices.IsTeamPlaying(playoffPlayerInfoEntity.C_Team);

                    _unitOfWork.PlayoffPlayerInfoRepository.Update(playoffPlayerInfo);


                }
                catch (ApplicationException ex)
                {
                    LogError.Write(ex,
                        $"ApplicationException in PlayerInfoServices.Create(IEnumerable<PlayoffPlayerInfoEntity> playoffPlayerInfoEntities)() for id: {playoffPlayerInfo.C_Name} and code: {playoffPlayerInfo.C_Code}");
                }
                catch (Exception ex)
                {
                    LogError.Write(ex,
                        $"Exception in PlayerInfoServices.Create(IEnumerable<PlayoffPlayerInfoEntity> playoffPlayerInfoEntities)() for id: {playoffPlayerInfo.C_Name} and code: {playoffPlayerInfo.C_Code}");
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
                        "Exception in PlayerInfoServices.Update(IEnumerable<PlayoffPlayerInfoEntity> playoffPlayerInfoEntities)()  on Save ALL to Context");
                updated = false;
                return updated;
            }

            return updated;
        }

        
        public bool UpdateFromMySportsFeeds(IEnumerable<PlayoffPlayerInfoEntity> playoffPlayerInfoEntities)
        {
            var updated = false;


            foreach (var playoffPlayerInfoEntity in playoffPlayerInfoEntities)
            {
                // via GEnRepo/UnitOFWork ctx
                var playoffPlayerInfo = _unitOfWork.PlayoffPlayerInfoRepository
                    .Get(x => x.C_Name == playoffPlayerInfoEntity.C_Name && x.C_Team == playoffPlayerInfoEntity.C_Team && x.C_Pos == playoffPlayerInfoEntity.C_Pos);

                // player does not exist
                if (playoffPlayerInfo == null)
                {
                    // create and continue
                    // TODO - Will have to find way to track exchange...Same concept. ID stays the same, but stats are split on the different team player stats page
                    // TODO - will have to do a web scrap to load CBC sport ID. Then if null check for id, if exists add points togheter see section old traded!
                    playoffPlayerInfoEntity.C_UserEmail = "";
                    playoffPlayerInfoEntity.C_TradedTeam = "";
                    Create(playoffPlayerInfoEntity); // todo fix the data inserted
                    continue;
                }
                try
                {
                    #region old traded not in use anymore. Check by name/pos/team for unique. if not found then create
                    //    // #1 Check if player was traded... and only one in DB (meaning newly traded team player not in DB yet - !! 1st update instance since the trade occur!!)
                    //    // we compare with the only retrive yet in db
                    //    if (playoffPlayerInfoEntity.C_Team != playoffPlayerInfo.C_Team)
                    //    {
                    //        // make sure the newly traded team player not in DB yet
                    //        if (!ExistWithTrade(playoffPlayerInfoEntity))
                    //        {
                    //            // get traded player and user details
                    //            playoffPlayerInfoEntity.C_UserEmail = playoffPlayerInfo.C_UserEmail;
                    //            playoffPlayerInfoEntity.I_Round = playoffPlayerInfo.I_Round;

                    //            playoffPlayerInfoEntity.I_PtLastD =
                    //                _pastPlayerInfoServices.GetYesterdayWhere(playoffPlayerInfoEntity.I_ApiId);
                    //            playoffPlayerInfoEntity.I_PtLastW = _pastPlayerInfoServices.GetWeekWhere(playoffPlayerInfoEntity.I_ApiId);
                    //            playoffPlayerInfoEntity.I_PtLastM = _pastPlayerInfoServices.GetMonthWhere(playoffPlayerInfoEntity.I_ApiId);

                    //            Create(playoffPlayerInfoEntity);
                    //            //LogError.Write(new ApplicationException("Traded"),
                    //                //$"This player has been traded {playoffPlayerInfoEntity.C_Name} to {playoffPlayerInfoEntity.C_Team} ");

                    //            // update traded previous team player with new team in C_TradedTeam and FLAG has traded
                    //            playoffPlayerInfo.C_TradedTeam = playoffPlayerInfoEntity.C_Team;
                    //            playoffPlayerInfo.L_Traded = true;
                    //            _unitOfWork.PlayoffPlayerInfoRepository.Update(playoffPlayerInfo);
                    //            _unitOfWork.Save(); 
                    //            continue;
                    //        }


                    //    }
                    //    // #2 Check if many player then get the one with new team. Then update his stats plus the one from traded player with old team
                    //    var playoffPlayerInfoList =
                    //        _unitOfWork.PlayoffPlayerInfoRepository.GetMany(p => p.I_ApiId == playoffPlayerInfoEntity.I_ApiId).ToList();
                    //    if (playoffPlayerInfoList.Count > 1)
                    //    {
                    //        var tradedPlayer =
                    //            playoffPlayerInfoList.FirstOrDefault(x => x.L_Traded == true && x.C_TradedTeam != String.Empty);
                    //        if (tradedPlayer == null)
                    //        {
                    //            LogError.Write(new Exception("Issue in update stats traded"), playoffPlayerInfoList.ToString());
                    //            continue;
                    //        }

                    //        // A) check if the playoffPlayerInfoEntity passed in is the traded player exit this Update() if it is
                    //        if (tradedPlayer.C_Team != playoffPlayerInfoEntity.C_Team)
                    //            continue;

                    //        // B) otherwise the playoffPlayerInfo passed in is the new traded player then update his stats
                    //        var newTeamPlayer =
                    //            playoffPlayerInfoList.FirstOrDefault(x => x.L_Traded == false && x.C_TradedTeam == String.Empty);
                    //        if (newTeamPlayer == null)
                    //        {
                    //            LogError.Write(new Exception("Issue in update stats traded"), playoffPlayerInfoList.ToString());
                    //            continue;
                    //        }

                    //        // Meaning: inNewTeamPlayer.I_Game = (newUpdatedData.I_Game + ConstantNoFutureUpdateInDbTradedPlayer.I_Game);
                    //        // Basically cheating incorrectness of the api to store correct values


                    //        newTeamPlayer.I_Game = (playoffPlayerInfoEntity.I_Game + tradedPlayer.I_Game);
                    //        newTeamPlayer.I_Goal = (playoffPlayerInfoEntity.I_Goal + tradedPlayer.I_Goal);
                    //        newTeamPlayer.I_Assist = (playoffPlayerInfoEntity.I_Assist + tradedPlayer.I_Assist);
                    //        newTeamPlayer.I_Point = (playoffPlayerInfoEntity.I_Point + tradedPlayer.I_Point);
                    //        newTeamPlayer.C_Toi = playoffPlayerInfoEntity.C_Toi;
                    //        newTeamPlayer.I_PpP = (playoffPlayerInfoEntity.I_PpP + tradedPlayer.I_PpP);
                    //        newTeamPlayer.I_ShP = (playoffPlayerInfoEntity.I_ShP + tradedPlayer.I_ShP);
                    //        newTeamPlayer.I_GwG = (playoffPlayerInfoEntity.I_GwG + tradedPlayer.I_GwG);
                    //        newTeamPlayer.I_OtG = (playoffPlayerInfoEntity.I_OtG + tradedPlayer.I_OtG);
                    //        newTeamPlayer.I_PtLastD = _pastPlayerInfoServices.GetYesterdayWhere(playoffPlayerInfoEntity.I_ApiId
                    //            /*, newTeamPlayer.I_Point*/);
                    //        newTeamPlayer.I_PtLastW = _pastPlayerInfoServices.GetWeekWhere(playoffPlayerInfoEntity.I_ApiId
                    //            /*, newTeamPlayer.I_Point*/);
                    //        newTeamPlayer.I_PtLastM = _pastPlayerInfoServices.GetMonthWhere(playoffPlayerInfoEntity.I_ApiId
                    //            /*, newTeamPlayer.I_Point*/);
                    //        //newTeamPlayer.L_IsPlaying = _teamScheduleServices.IsTeamPlaying(playoffPlayerInfoEntity.C_Team);
                    //        _unitOfWork.PlayoffPlayerInfoRepository.Update(newTeamPlayer);//_bulkCtxEntities.Entry(newTeamPlayer).State = EntityState.Modified;//
                    //        _unitOfWork.Save();//_bulkCtxEntities.SaveChanges();//

                    //        //updated = true;
                    //        continue;
                    //    }

                    #endregion
                    // Necessary stats for from NHL API
                    playoffPlayerInfo.I_Game = playoffPlayerInfoEntity.I_Game;
                    playoffPlayerInfo.I_Goal = playoffPlayerInfoEntity.I_Goal;
                    playoffPlayerInfo.I_Assist = playoffPlayerInfoEntity.I_Assist;
                    playoffPlayerInfo.I_Point = playoffPlayerInfoEntity.I_Point;
                    playoffPlayerInfo.C_Toi = playoffPlayerInfoEntity.C_Toi;
                    playoffPlayerInfo.I_PpP = playoffPlayerInfoEntity.I_PpP;
                    playoffPlayerInfo.I_ShP = playoffPlayerInfoEntity.I_ShP;

                    // removed with Scarpping
                    //playoffPlayerInfo.I_GwG = playoffPlayerInfoEntity.I_GwG;
                    //playoffPlayerInfo.I_OtG = playoffPlayerInfoEntity.I_OtG;

                    playoffPlayerInfo.I_PtLastD = _pastPlayerInfoServices.GetYesterdayWhere(playoffPlayerInfoEntity);
                    playoffPlayerInfo.I_PtLastW = _pastPlayerInfoServices.GetWeekWhere(playoffPlayerInfoEntity);
                    playoffPlayerInfo.I_PtLastM = _pastPlayerInfoServices.GetMonthWhere(playoffPlayerInfoEntity);
                    //playoffPlayerInfo.L_IsPlaying = _teamScheduleServices.IsTeamPlaying(playoffPlayerInfoEntity.C_Team);

                    _unitOfWork.PlayoffPlayerInfoRepository.Update(playoffPlayerInfo);


                }
                catch (ApplicationException ex)
                {
                    LogError.Write(ex,
                        $"ApplicationException in PlayerInfoServices.Create(IEnumerable<PlayoffPlayerInfoEntity> playoffPlayerInfoEntities)() for id: {playoffPlayerInfo.C_Name} and code: {playoffPlayerInfo.C_Code}");
                }
                catch (Exception ex)
                {
                    LogError.Write(ex,
                        $"Exception in PlayerInfoServices.Create(IEnumerable<PlayoffPlayerInfoEntity> playoffPlayerInfoEntities)() for id: {playoffPlayerInfo.C_Name} and code: {playoffPlayerInfo.C_Code}");
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
                        "Exception in PlayerInfoServices.Update(IEnumerable<PlayoffPlayerInfoEntity> playoffPlayerInfoEntities)()  on Save ALL to Context");
                updated = false;
                return updated;
            }

            return updated;
        }

        public bool UpdateAvg()
        {
            var players = _unitOfWork.PlayoffPlayerInfoRepository.GetMany(p => p.I_Point > 0).ToList();
            if (!players.Any())
            {
                LogError.Write(new Exception("No players found form DB.."), "Exception in UpdateAvg ");
                return false;
            }
            foreach (var playoffPlayerInfo in players)
            {
                try
                {
                    var game = playoffPlayerInfo.I_Game;
                    var point = (double)playoffPlayerInfo.I_Point;
                    var avg = point / game;
                    playoffPlayerInfo.F_Avg = Math.Round(avg, 2);
                    _unitOfWork.PlayoffPlayerInfoRepository.Update(playoffPlayerInfo);
                    _unitOfWork.Save();
                }
                catch (Exception ex)
                {
                    LogError.Write(ex, "Exception in UpdateAvg at player code: " + playoffPlayerInfo.C_Code);
                }
            }

            return true;
        }


        // @@@@ Just use same query to exec all 3 uptate
        public bool UpdateStatus()
        {
            _unitOfWork.ClearAllStatus();
            var players = _unitOfWork.PlayoffPlayerInfoRepository.GetMany(p => p.C_UserEmail != string.Empty).ToList();
            if (!players.Any())
            {
                LogError.Write(new Exception("No players found form DB.."), "Exception in Update status ");
                return false;
            }

            // REMOVE OUT FOR PLAYOFF!
            //var users = players.Select(x => x.C_UserEmail).Distinct().ToList();
            //foreach (var user in users)
            //{

            //    //Statuses.Out = 3. Get 2 worst by user
            //    try
            //    {
            //        var worst2Players =
            //            players.Where(p => p.C_UserEmail == user && p.L_Traded == false)
            //                .OrderBy(p => p.I_Point) // less to most
            //                .ThenByDescending(p => p.I_Game) // most to less
            //                .Take(2).ToList();
            //        var playerOut1 = worst2Players.ElementAt(0);
            //        playerOut1.I_Status = (int)Statuses.Out;
            //        var playerOut2 = worst2Players.ElementAt(1);
            //        playerOut2.I_Status = (int)Statuses.Out;
            //        _unitOfWork.PlayoffPlayerInfoRepository.Update(playerOut1);
            //        _unitOfWork.PlayoffPlayerInfoRepository.Update(playerOut2);
            //        _unitOfWork.Save();
            //    }
            //    catch (Exception ex)
            //    {
            //        LogError.Write(ex, "Exception in UpdateStatus at user: " + user);
            //    }
            //}

            //Statuses.Top = 1, check if for a given user if one of his picks is top of round
            for (int i = 1; i <= Constants.Rules.PlayoffRounds; i++)
            {
                try
                {
                    var player = players.Where(p => p.I_Round == i && p.L_Traded == false)
                        .OrderByDescending(p => p.I_Point) // most point
                        .ThenBy(p => p.I_Game) // with less game
                        .Take(1).ToList();
                    var topOfRound = player.ElementAt(0);
                    topOfRound.I_Status = (int)Statuses.Top;
                    _unitOfWork.PlayoffPlayerInfoRepository.Update(topOfRound);
                    _unitOfWork.Save();
                }
                catch (Exception ex)
                {
                    LogError.Write(ex, "Exception in UpdateStatus at top round: " + i);
                }
            }

            //Statuses.Worst = 2, check if for a given user if one of his picks is worst of round
            for (int i = 1; i <= Constants.Rules.PlayoffRounds; i++)
            {
                try
                {
                    var player = players.Where(p => p.I_Round == i && p.L_Traded == false && p.I_Status != (int)Statuses.Out)
                        .OrderBy(p => p.I_Point) // less point
                        .ThenByDescending(p => p.I_Game) // with most game
                        .Take(1).ToList();
                    var worstOfRound = player.ElementAt(0);
                    worstOfRound.I_Status = (int)Statuses.Worst;
                    _unitOfWork.PlayoffPlayerInfoRepository.Update(worstOfRound);
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
            var playoffPlayerInfoEntities = _unitOfWork.PlayoffPlayerInfoRepository.GetAll().ToList();

            foreach (var injured in injuryList)
            {
                try
                {
                    var playoffPlayerInfo = playoffPlayerInfoEntities.FirstOrDefault(p => p.C_Name == injured.Name);// new api: change compare name only since using full name... removed: && p.C_Pos == injured.Position);
                    if (playoffPlayerInfo == null) continue;

                    playoffPlayerInfo.L_IsInjured = true;
                    playoffPlayerInfo.C_InjStatus = injured.Status;
                    playoffPlayerInfo.C_InjDetails = injured.Details;
                    _unitOfWork.PlayoffPlayerInfoRepository.Update(playoffPlayerInfo);
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

        public bool Delete(string playoffPlayerInfoCode)
        {
            throw new NotImplementedException();
        }

        public bool Exist(PlayoffPlayerInfoEntity playoffPlayerInfoEntity)
        {
            var playerExist = _unitOfWork.PlayoffPlayerInfoRepository.Get(x => x.C_Name == playoffPlayerInfoEntity.C_Name && x.C_Team == playoffPlayerInfoEntity.C_Team && x.C_Pos == playoffPlayerInfoEntity.C_Pos);
            return playerExist != null;
        }

        ///// <summary>
        ///// Retrun true if many apiId already in dB
        ///// </summary>
        ///// <param name="playoffPlayerInfoEntity"></param>
        ///// <returns></returns>
        //public bool ExistWithTrade(PlayoffPlayerInfoEntity playoffPlayerInfoEntity)
        //{
        //    var playerExists = _unitOfWork.PlayoffPlayerInfoRepository.GetMany(p => p.I_ApiId == playoffPlayerInfoEntity.I_ApiId);
        //    if (playerExists.Count() > 1)
        //        return true;
        //    return false;
        //}
    }
}
