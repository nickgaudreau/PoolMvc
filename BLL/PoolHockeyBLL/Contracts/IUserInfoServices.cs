﻿using System.Collections.Generic;
using PoolHockeyBLL.BizEntities;

namespace PoolHockeyBLL.Contracts
{
    /// <summary>
    /// UserInfo Service Contract
    /// </summary>
    public interface IUserInfoServices
    {
        UserInfoEntity GetByEmail(string email);
        UserInfoEntity GetTopBestDay();
        //UserInfoEntity GetBestLastD();
        void UpdateBestDay();
        void UpdateBestMonth();
        //void UpdateBestLastD();
    }
}