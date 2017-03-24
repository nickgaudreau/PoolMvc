using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using PoolHockeyBLL.BizEntities;
using PoolHockeyBLL.Contracts;
using PoolHockeyBLL.Log;
using PoolHockeyBOL;
using PoolHockeyDAL.UnitOfWork;

namespace PoolHockeyBLL
{
    public class UserInfoServices : IUserInfoServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICaching _caching;
        private readonly IPastPlayerInfoServices _pastPlayerInfoServices;

        public UserInfoServices(UnitOfWork unitOfWork, IPastPlayerInfoServices pastPlayerInfoServices)
        {
            _unitOfWork = unitOfWork;
            _caching = new Caching();
            _pastPlayerInfoServices = pastPlayerInfoServices; 
        }

        public UserInfoEntity GetByEmail(string email)
        {
            UserInfo userInfo;
            try
            {
                userInfo = _unitOfWork.UserInfoRepository.GetSingle(u => u.C_UserEmail == email).Result;
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "More than one user with this email: " + email);
                return null;
            }            if (userInfo == null) return null;

            Mapper.CreateMap<UserInfo, UserInfoEntity>();
            var userInfoEntity = Mapper.Map<UserInfo, UserInfoEntity>(userInfo);

            return userInfoEntity;
        }

        public UserInfoEntity GetTopBestDay()
        {
            var userInfoCache = (IEnumerable<UserInfo>)_caching.GetCachedItem("UserInfoGetAll");

            UserInfo userInfoTopDay;

            if (userInfoCache == null)
            {
                userInfoCache = _unitOfWork.UserInfoRepository.GetAll().Result;
                _caching.AddToCache("UserInfoGetAll", userInfoCache);
                userInfoTopDay = userInfoCache.OrderByDescending(u => u.I_BestDay).FirstOrDefault();
                if (userInfoTopDay == null) return null;
            }
            else
            {
                userInfoTopDay = userInfoCache.OrderByDescending(u => u.I_BestDay).FirstOrDefault();
            }

            Mapper.CreateMap<UserInfo, UserInfoEntity>();
            var userInfoEntity = Mapper.Map<UserInfo, UserInfoEntity>(userInfoTopDay);

            return userInfoEntity;
        }

        public UserInfoEntity GetTopBestMonth()
        {
            var userInfoCache = (IEnumerable<UserInfo>)_caching.GetCachedItem("UserInfoGetAll");

            UserInfo userInfoTopMonth;

            if (userInfoCache == null)
            {
                userInfoCache = _unitOfWork.UserInfoRepository.GetAll().Result;
                _caching.AddToCache("UserInfoGetAll", userInfoCache);
                userInfoTopMonth = userInfoCache.OrderByDescending(u => u.I_BestMonth).FirstOrDefault();
                if (userInfoTopMonth == null) return null;
            }
            else
            {
                userInfoTopMonth = userInfoCache.OrderByDescending(u => u.I_BestMonth).FirstOrDefault();
            }

            Mapper.CreateMap<UserInfo, UserInfoEntity>();
            var userInfoEntity = Mapper.Map<UserInfo, UserInfoEntity>(userInfoTopMonth);

            return userInfoEntity;
        }

        public IEnumerable<UserInfoEntity> GetAll()
        {
            var userInfoCache = (IEnumerable<UserInfo>)_caching.GetCachedItem("UserInfoGetAll");

            IEnumerable<UserInfo> userInfo = null;

            if (userInfoCache == null)
            {
                userInfo = _unitOfWork.UserInfoRepository.GetAll().Result;
                _caching.AddToCache("UserInfoGetAll", userInfo);
                if (!userInfo.Any()) return null;
            }
            else
            {
                userInfo = userInfoCache;
            }

            Mapper.CreateMap<UserInfo, UserInfoEntity>();
            var userInfoEntities = Mapper.Map<List<UserInfo>, List<UserInfoEntity>>((List<UserInfo>)userInfo);

            return userInfoEntities;
        }

        // not in use..
        public IEnumerable<UserInfoEntity> GetAllWhere(string userEmail)
        {
            var userInfos = _unitOfWork.UserInfoRepository.GetManyQueryable(u => u.C_UserEmail == userEmail).ToList();            if (!userInfos.Any()) return null;

            Mapper.CreateMap<UserInfo, UserInfoEntity>();
            var userInfoEntities = Mapper.Map<List<UserInfo>, List<UserInfoEntity>>(userInfos);

            return userInfoEntities;
        }

        // not in contract
        internal int GetMonthlyPointsForUser(string usermail)
        {
            var playerInfo = _unitOfWork.PlayerInfoRepository.GetManyQueryable(p => p.C_UserEmail == usermail && p.I_Status != (int)Statuses.Out);
            //var date = DateTime.Now;

            var total = 0;
            foreach (var player in playerInfo)
            {
                var pointsLastMonth = _pastPlayerInfoServices.GetActualMonthWhere(player);

                total += pointsLastMonth; //pastPlayerInfoLastDay.I_Point - pastPlayerInfoFirstDay.I_Point;
            }
            return total;
        }

        public bool Create(UserInfoEntity userInfoEntity)
        {
            bool created;
            try
            {
                using (var scope = new TransactionScope())
                {
                    Mapper.CreateMap<UserInfoEntity, UserInfo>();
                    var userInfo = Mapper.Map<UserInfoEntity, UserInfo>(userInfoEntity);

                    _unitOfWork.UserInfoRepository.Insert(userInfo);
                    _unitOfWork.Save();
                    scope.Complete();
                    created = true;
                }
            }
            catch (TransactionAbortedException ex)
            {
                created = false;
                LogError.Write(ex, "TransactionAbortedException");

            }
            catch (ApplicationException ex)
            {
                created = false;
                LogError.Write(ex, "ApplicationException");
            }
            catch (Exception ex)
            {
                created = false;
                LogError.Write(ex, "Exception");
            }

            return created;
        }

        public bool Update(UserInfoEntity userInfoEntity, string code)
        {
            var updated = false;

            try
            {
                using (var scope = new TransactionScope())
                {
                    var userInfo = _unitOfWork.UserInfoRepository.GetByID(code).Result;
                    if (userInfo == null) return updated;

                    // Necessary stats for from NHL API
                    userInfo.I_Games = userInfoEntity.I_Games;
                    userInfo.I_Goals = userInfoEntity.I_Goals;
                    userInfo.I_Assists = userInfoEntity.I_Assists;
                    userInfo.I_Points = userInfoEntity.I_Points;
                    userInfo.I_PtLastD = userInfoEntity.I_PtLastD;
                    userInfo.I_PtLastW = userInfoEntity.I_PtLastW;
                    userInfo.I_PtLastM = userInfoEntity.I_PtLastM;


                    _unitOfWork.UserInfoRepository.Update(userInfo);
                    _unitOfWork.Save();
                    scope.Complete();
                    updated = true;
                }
            }
            catch (TransactionAbortedException ex)
            {
                LogError.Write(ex, "TransactionAbortedException");

            }
            catch (ApplicationException ex)
            {
                LogError.Write(ex, "ApplicationException");
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Exception");
            }

            return updated;
        }

        public bool UpdateAll()
        {
            var updated = false;

            try
            {
                var users = _unitOfWork.UserInfoRepository.GetAll().Result;
                if (!users.Any())
                {
                    LogError.Write(new Exception("Error"), "UserInfo Get all returned 0 values");
                    return false;
                }
                foreach (var userInfo in users)
                {
                    var userStats = _unitOfWork.PlayerInfoRepository
                        .GetManyQueryable(p => p.C_UserEmail == userInfo.C_UserEmail && p.I_Status != (int)Statuses.Out);
                    if (!userStats.Any())
                    {
                        LogError.Write(new Exception("Error"), "Could not retrieve user stats for this user info in PlayerInfo Table");
                        continue;
                    }

                    var userInfoEntity = new UserInfoEntity
                    {
                        I_Games = userStats.Sum(u => u.I_Game),
                        I_Goals = userStats.Sum(u => u.I_Goal),
                        I_Assists = userStats.Sum(u => u.I_Assist),
                        I_Points = userStats.Sum(u => u.I_Point),
                        I_PtLastD = userStats.Sum(u => u.I_PtLastD),
                        I_PtLastM = userStats.Sum(u => u.I_PtLastM),
                        I_PtLastW = userStats.Sum(u => u.I_PtLastW)
                    };

                    Update(userInfoEntity, userInfo.C_Code);

                }
                updated = true;
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Issue Update All");
            }

            return updated;
        }

        public void UpdateBestDay()
        {
            var playerInfo = _unitOfWork
                .PlayerInfoRepository
                .GetManyQueryable(p => p.C_UserEmail.Length > 0 && p.I_Status != (int)Statuses.Out);


            var userInfo = _unitOfWork.UserInfoRepository.GetAll().Result;

            foreach (var user in userInfo)
            {
                var pastBestDay = user.I_BestDay;

                var yesterdayPoints = playerInfo.Where(p => p.C_UserEmail == user.C_UserEmail).Sum(x => x.I_PtLastD);

                if (yesterdayPoints <= pastBestDay) continue;

                user.I_BestDay = yesterdayPoints;
                user.D_BestDay = DateTime.Now.AddDays(-1);
                try
                {
                    _unitOfWork.UserInfoRepository.Update(user);
                    _unitOfWork.Save();
                }
                catch (TransactionAbortedException ex)
                {
                    LogError.Write(ex, "TransactionAbortedException");

                }
                catch (ApplicationException ex)
                {
                    LogError.Write(ex, "ApplicationException");
                }
                catch (Exception ex)
                {
                    LogError.Write(ex, "Exception");
                }
            }

        }

        public void UpdateBestMonth()
        {
            var userInfo = _unitOfWork.UserInfoRepository.GetAll().Result;

            foreach (var user in userInfo)
            {
                var pastBestMonth = user.I_BestMonth;

                var totalPointsThisMonth = GetMonthlyPointsForUser(user.C_UserEmail);

                if (totalPointsThisMonth <= pastBestMonth) continue;

                user.I_BestMonth = totalPointsThisMonth;
                user.D_BestMonth = DateTime.Now.AddDays(-1);
                try
                {
                    _unitOfWork.UserInfoRepository.Update(user);
                    _unitOfWork.Save();
                }
                catch (TransactionAbortedException ex)
                {
                    LogError.Write(ex, "TransactionAbortedException");

                }
                catch (ApplicationException ex)
                {
                    LogError.Write(ex, "ApplicationException");
                }
                catch (Exception ex)
                {
                    LogError.Write(ex, "Exception");
                }
            }
        }

        public bool Delete(string userInfoCode)
        {
            throw new NotImplementedException();
        }


    }
}
