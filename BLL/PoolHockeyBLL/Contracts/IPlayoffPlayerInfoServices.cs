using System.Collections.Generic;
using PoolHockeyBLL.BizEntities;

namespace PoolHockeyBLL.Contracts
{
    public interface IPlayoffPlayerInfoServices
    {
        PlayoffPlayerInfoEntity GetById(string playoffPlayerInfoCode);        IEnumerable<PlayoffPlayerInfoEntity> GetAll();
        IEnumerable<PlayoffPlayerInfoEntity> GetUndrafted();
        IEnumerable<PlayoffPlayerInfoEntity> GetLeagueLeaders();
        IEnumerable<PlayoffPlayerInfoEntity> GetInjured();
        IEnumerable<PlayoffPlayerInfoEntity> GetBestPerRound(int round);        IEnumerable<PlayoffPlayerInfoEntity> GetAllWhere(string userEmail);        bool Create(PlayoffPlayerInfoEntity playoffPlayerInfoEntity);
        bool CreateList(List<PlayoffPlayerInfoEntity> playoffPlayerInfoEntities);        bool Update(IEnumerable<PlayoffPlayerInfoEntity> playoffPlayerInfoEntities);
        bool UpdateFromMySportsFeeds(IEnumerable<PlayoffPlayerInfoEntity> playoffPlayerInfoEntities);
        bool UpdateAvg();
        bool UpdateStatus();
        bool UpdateInjuryStatus();        bool Delete(string playoffPlayerInfoCode);
        bool Exist(PlayoffPlayerInfoEntity playoffPlayerInfoEntity);
    }
}
