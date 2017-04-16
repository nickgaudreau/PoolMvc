using System.Collections.Generic;
using PoolHockeyBLL.BizEntities;
using PoolHockeyBOL;

namespace PoolHockeyBLL.Contracts
{
    /// <summary>
    /// PastPlayerInfo Service Contract
    /// </summary>
    public interface IPastPlayerInfoServices
    {        int GetYesterdayWhere(IPlayerEntity playerInfoEntity);
        int GetWeekWhere(IPlayerEntity playerInfoEntity);
        int GetMonthWhere(IPlayerEntity playerInfoEntity);        int GetActualMonthWhere(IPlayerEntity playerInfoEntity);        bool Create(IEnumerable<IPlayerEntity> playerInfoEntities);
        //bool Exist(PlayerInfo playerInfo);


    }
}
