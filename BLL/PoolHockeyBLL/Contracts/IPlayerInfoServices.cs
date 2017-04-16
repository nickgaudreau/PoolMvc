using System.Collections.Generic;
using PoolHockeyBLL.BizEntities;

namespace PoolHockeyBLL.Contracts
{
    /// <summary>
    /// PlayerInfo Service Contract
    /// </summary>
    public interface IPlayerInfoServices
    {
        PlayerInfoEntity GetById(string playerInfoCode);        IEnumerable<PlayerInfoEntity> GetAll();
        IEnumerable<PlayerInfoEntity> GetUndrafted();
        IEnumerable<PlayerInfoEntity> GetLeagueLeaders();
        IEnumerable<PlayerInfoEntity> GetInjured();
        IEnumerable<PlayerInfoEntity> GetBestPerRound(int round);        IEnumerable<PlayerInfoEntity> GetAllWhere(string userEmail);        bool Create(PlayerInfoEntity playerInfoEntity);
        bool CreateList(List<PlayerInfoEntity> playerInfoEntities);        bool Update(IEnumerable<PlayerInfoEntity> playerInfoEntities);
        bool UpdateFromMySportsFeeds(IEnumerable<PlayerInfoEntity> playerInfoEntities);
        bool UpdateAvg();
        bool UpdateStatus();
        bool UpdateInjuryStatus();        bool Delete(string playerInfoCode);
        bool Exist(PlayerInfoEntity playerInfoEntity);
        //bool ExistWithTrade(PlayerInfoEntity playerInfoEntity);


        // To implemenbt in Services with try catch... so can be called form controller!


        // if use Web API => AsQueryable();
        ///////// <summary>
        ///////// generic method to get many record on the basis of a condition but query able.
        ///////// </summary>
        ///////// <param name="where"></param>
        ///////// <returns></returns>
        //////public virtual IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where)
        //////{
        //////    return DbSet.Where(where).AsQueryable();
        //////}
    }
}
