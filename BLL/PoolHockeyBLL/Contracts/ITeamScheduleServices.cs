using System;
using PoolHockeyBOL;

namespace PoolHockeyBLL.Contracts
{
    public interface ITeamScheduleServices
    {
        bool IsTeamPlaying(string team);
        bool WasTeamPlaying(string team);
        bool Create(TeamSchedule teamSchedule);
        bool Update();
    }
}
