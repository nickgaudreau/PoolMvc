﻿using System.Collections.Generic;
using PoolHockeyBLL.BizEntities;
using PoolHockeyBOL;

namespace PoolHockeyBLL.Contracts
{
    /// <summary>
    /// PastPlayerInfo Service Contract
    /// </summary>
    public interface IPastPlayerInfoServices
    {
        int GetWeekWhere(PlayerInfoEntity playerInfoEntity);
        int GetMonthWhere(PlayerInfoEntity playerInfoEntity);
        bool Exist(PlayerInfo playerInfo);


    }
}