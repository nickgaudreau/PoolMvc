using System;
using System.Transactions;
using PoolHockeyBLL.Contracts;
using PoolHockeyBLL.Log;
using PoolHockeyBOL;
using PoolHockeyDAL.UnitOfWork;

namespace PoolHockeyBLL
{
    public class TeamScheduleServices : ITeamScheduleServices
    {
        private readonly IUnitOfWork _unitOfWork;

        ///// <summary>
        ///// For internal use
        ///// </summary>
        //internal TeamScheduleServices() {  }

        public TeamScheduleServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; //new UnitOfWork();
        }

        public bool IsTeamPlaying(string team)
        {
            var date = DateTime.Today;
            return _unitOfWork.TeamScheduleRepository.Get(x => x.C_Team == team && x.D_Date == date) != null;
        }

        public bool WasTeamPlaying(string team)
        {
            var date = DateTime.Today.AddDays(-1);
            return _unitOfWork.TeamScheduleRepository.Get(x => x.C_Team == team && x.D_Date == date) != null;
        }

        public bool Create(TeamSchedule teamSchedule)
        {
            bool created;
            try
            {
                using (var scope = new TransactionScope())
                {
                    //Mapper.CreateMap<UserInfoEntity, UserInfo>();
                    //var userInfo = Mapper.Map<UserInfoEntity, UserInfo>(userInfoEntity);

                    _unitOfWork.TeamScheduleRepository.Insert(teamSchedule);
                    _unitOfWork.Save();
                    scope.Complete();
                    created = true;
                }
            }
            catch (TransactionAbortedException ex)
            {
                created = false;
                LogError.Write(ex, "TransactionAbortedException");

            }
            catch (ApplicationException ex)
            {
                created = false;
                LogError.Write(ex, "ApplicationException");
            }
            catch (Exception ex)
            {
                created = false;
                LogError.Write(ex, "Exception");
            }

            return created;
        }

        public bool Update()
        {

            var updated = false;
            var teamPlayingToday = _unitOfWork.TeamScheduleRepository.GetManyQueryable(x => x.D_Date == DateTime.Today);
            _unitOfWork.ClearPlayingToday();
            foreach (var team in teamPlayingToday)
            {
                // this would be better with ADO.NET = Update all where
                // lets use a store proc
                _unitOfWork.SetPlayingToday(team.C_Team);
            }

            return updated;

        }
    }
}
