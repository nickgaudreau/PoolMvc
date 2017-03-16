using System.Collections.Generic;
using PoolHockeyBLL.BizEntities;
using PoolHockeyBOL;

namespace PoolHockeyBLL.Contracts
{
    /// <summary>
    /// PastPlayerInfo Service Contract
    /// </summary>
    public interface IPastPlayerInfoServices
    {        int GetYesterdayWhere(PlayerInfoEntity playerInfoEntity);
        int GetWeekWhere(PlayerInfoEntity playerInfoEntity);
        int GetMonthWhere(PlayerInfoEntity playerInfoEntity);        int GetActualMonthWhere(PlayerInfo playerInfoEntity);        bool Create(IEnumerable<PlayerInfoEntity> playerInfoEntities);
        bool Exist(PlayerInfo playerInfo);


    }
}
