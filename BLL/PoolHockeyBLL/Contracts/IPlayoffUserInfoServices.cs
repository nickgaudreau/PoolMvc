﻿using System.Collections.Generic;
using PoolHockeyBLL.BizEntities;

namespace PoolHockeyBLL.Contracts
{
    /// <summary>
    /// IPlayoffPlayoffUser Service Contract
    /// </summary>
    public interface IPlayoffUserInfoServices
    {
        PlayoffUserInfoEntity GetByEmail(string email);
        PlayoffUserInfoEntity GetTopBestDay();
        //PlayoffUserInfoEntity GetBestLastD();
        void UpdateBestDay();
        void UpdateBestMonth();
        //void UpdateBestLastD();
    }
}