﻿using System.Collections.Generic;
using PoolHockeyBLL.BizEntities;

namespace PoolHockeyBLL.Contracts
{
    public interface IPlayoffPlayerInfoServices
    {
        PlayoffPlayerInfoEntity GetById(string playoffPlayerInfoCode);
        IEnumerable<PlayoffPlayerInfoEntity> GetUndrafted();
        IEnumerable<PlayoffPlayerInfoEntity> GetLeagueLeaders();
        IEnumerable<PlayoffPlayerInfoEntity> GetInjured();
        IEnumerable<PlayoffPlayerInfoEntity> GetBestPerRound(int round);
        bool CreateList(List<PlayoffPlayerInfoEntity> playoffPlayerInfoEntities);
        bool UpdateFromMySportsFeeds(IEnumerable<PlayoffPlayerInfoEntity> playoffPlayerInfoEntities);
        bool UpdateAvg();
        bool UpdateStatus();
        bool UpdateInjuryStatus();
        bool Exist(PlayoffPlayerInfoEntity playoffPlayerInfoEntity);
    }
}