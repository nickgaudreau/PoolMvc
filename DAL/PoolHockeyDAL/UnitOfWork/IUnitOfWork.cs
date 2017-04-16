using System;
using PoolHockeyBOL;

namespace PoolHockeyDAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        // Db accessor
        GenericRepository<PastPlayerInfo> PastPlayerInfoRepository { get; }
        GenericRepository<PlayerInfo> PlayerInfoRepository { get; }
        GenericRepository<PoolLastYear> PoolLastYearRepository { get;}
        GenericRepository<TeamSchedule> TeamScheduleRepository { get; }
        GenericRepository<UserInfo> UserInfoRepository { get; }
        GenericRepository<PlayoffPlayerInfo> PlayoffPlayerInfoRepository { get; }
        GenericRepository<PlayoffUserInfo> PlayoffUserInfoRepository { get; }

        /// <summary>
        /// Save method.
        /// </summary>
        void Save();

        // Store PRoc
        void ClearPlayingToday();
        void SetPlayingToday(string s);
        void ClearAllStatus();
        void ClearAllInjuryStatus();
        DateTime GetLastUpdate();
        void SetLastUpdate(DateTime d);
    }
}
