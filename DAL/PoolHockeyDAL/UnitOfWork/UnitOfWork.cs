using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using PoolHockeyBOL;

namespace PoolHockeyDAL.UnitOfWork
{

    /// <summary>
    /// Unit of Work class responsible for DB transactions
    /// </summary>
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        #region Private member variables...
        private readonly PoolHockeyBOL.Entities _context = null;
        private GenericRepository<PlayerInfo> _playerInfoRepository;
        private GenericRepository<UserInfo> _userInfoRepository;
        private GenericRepository<UserFact> _userFactRepository;
        private GenericRepository<PastPlayerInfo> _pastPlayerInfoRepository;
        private GenericRepository<PoolLastYear> _poolLastYearRepository;
        private GenericRepository<TeamSchedule> _teamScheduleRepository;
        private GenericRepository<PlayoffPlayerInfo> _playoffPlayerInfoRepository;
        private GenericRepository<PlayoffUserInfo> _playoffUserInfoRepository;
        #endregion
        public UnitOfWork()
        {
            _context = new PoolHockeyBOL.Entities();
            _context.Configuration.AutoDetectChangesEnabled = false; // improve perfomance
        }

        #region Public Repository Creation properties...only if not already instantiated!
        /// <summary>
        /// Get/Set Property for PlayerInfo repository.
        /// </summary>
        public GenericRepository<PlayerInfo> PlayerInfoRepository
        {
            get
            {
                if (this._playerInfoRepository == null)
                    this._playerInfoRepository = new GenericRepository<PlayerInfo>(_context);
                return _playerInfoRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for UserInfo repository.
        /// </summary>
        public GenericRepository<UserInfo> UserInfoRepository
        {
            get
            {
                if (this._userInfoRepository == null)
                    this._userInfoRepository = new GenericRepository<UserInfo>(_context);
                return _userInfoRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for UserFact repository.
        /// </summary>
        public GenericRepository<UserFact> UserFactRepository
        {
            get
            {
                if (this._userFactRepository == null)
                    this._userFactRepository = new GenericRepository<UserFact>(_context);
                return _userFactRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for PastPlayerInfo repository.
        /// </summary>
        public GenericRepository<PastPlayerInfo> PastPlayerInfoRepository
        {
            get
            {
                if (this._pastPlayerInfoRepository == null)
                    this._pastPlayerInfoRepository = new GenericRepository<PastPlayerInfo>(_context);
                return _pastPlayerInfoRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for PastPlayerInfo repository.
        /// </summary>
        public GenericRepository<PoolLastYear> PoolLastYearRepository
        {
            get
            {
                if (this._poolLastYearRepository == null)
                    this._poolLastYearRepository = new GenericRepository<PoolLastYear>(_context);
                return _poolLastYearRepository;
            }
        }

        public GenericRepository<TeamSchedule> TeamScheduleRepository
        {
            get
            {
                if (this._teamScheduleRepository == null)
                    this._teamScheduleRepository = new GenericRepository<TeamSchedule>(_context);
                return _teamScheduleRepository;
            }
        }


        /// <summary>
        /// Get/Set Property for PlayoffPlayerInfo repository.
        /// </summary>
        public GenericRepository<PlayoffPlayerInfo> PlayoffPlayerInfoRepository
        {
            get
            {
                if (this._playoffPlayerInfoRepository == null)
                    this._playoffPlayerInfoRepository = new GenericRepository<PlayoffPlayerInfo>(_context);
                return _playoffPlayerInfoRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for playoff UserInfo repository.
        /// </summary>
        public GenericRepository<PlayoffUserInfo> PlayoffUserInfoRepository
        {
            get
            {
                if (this._playoffUserInfoRepository == null)
                    this._playoffUserInfoRepository = new GenericRepository<PlayoffUserInfo>(_context);
                return _playoffUserInfoRepository;
            }
        }

        /// <summary>
        /// Store proc : update all playerInfo status to 0 where 0 = none
        /// </summary>
        public void ClearAllStatus()
        {
            try
            {
                _context.ClearAllStatuses();
            }
            catch (Exception ex)
            {
                var list = new List<string>() { ex.Message + ". \n", ex.StackTrace + ". \n", ex.InnerException.ToString() + ". \n" };
                Log.LogError.WriteMany(ex, list);
                //System.IO.File.AppendAllLines(@"~\ef-errors.txt", list);
            }

        }

        /// <summary>
        /// Store proc : update all playerInfo injury status to 0
        /// </summary>
        public void ClearAllInjuryStatus()
        {
            try
            {
                _context.ClearAllInjuredStatuses();
            }
            catch (Exception ex)
            {
                var list = new List<string>() { ex.Message + ". \n", ex.StackTrace + ". \n", ex.InnerException.ToString() + ". \n" };
                Log.LogError.WriteMany(ex, list);
                //System.IO.File.AppendAllLines(@"~\ef-errors.txt", list);
            }

        }

        /// <summary>
        /// Store proc : clear all playerInfo l_playing to 0
        /// </summary>
        public void ClearPlayingToday()
        {
            try
            {
                _context.ClearPlayingToday();
            }
            catch (Exception ex)
            {
                var list = new List<string>() { ex.Message + ". \n", ex.StackTrace + ". \n", ex.InnerException.ToString() + ". \n" };
                Log.LogError.WriteMany(ex, list);
                //System.IO.File.AppendAllLines(@"~\ef-errors.txt", list);
            }

        }

        /// <summary>
        /// Store proc : update all playerInfo playing today
        /// </summary>
        public void SetPlayingToday(string team)
        {
            try
            {
                _context.SetPlayingToday(team);
            }
            catch (Exception ex)
            {
                var list = new List<string>() { ex.Message + ". \n", ex.StackTrace + ". \n", ex.InnerException.ToString() + ". \n" };
                Log.LogError.WriteMany(ex, list);
                //System.IO.File.AppendAllLines(@"~\ef-errors.txt", list);
            }

        }

        public DateTime GetLastUpdate()
        {
            try
            {
                var dateTime = _context.Configs.First().T_LastUpdate; // simplistic no use of Genric Repo <TEntity>
                return dateTime;
            }
            catch (Exception ex)
            {
                var list = new List<string>() { ex.Message + ". \n", ex.StackTrace + ". \n", ex.InnerException.ToString() + ". \n" };
                Log.LogError.WriteMany(ex, list);
                //System.IO.File.AppendAllLines(@"~\ef-errors.txt", list);
            }

            return DateTime.Now;
        }

        public void SetLastUpdate(DateTime dateTime)
        {
            try
            {
                _context.SetLastUpdate(dateTime);
            }
            catch (Exception ex)
            {
                var list = new List<string>() { ex.Message + ". \n", ex.StackTrace + ". \n", ex.InnerException.ToString() + ". \n" };
                Log.LogError.WriteMany(ex, list);
                //System.IO.File.AppendAllLines(@"~\ef-errors.txt", list);
            }

        }

        #endregion
        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now,
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                Log.LogError.WriteMany(e, outputLines);
                //System.IO.File.AppendAllLines(@"~\ef-errors.txt", outputLines);
                throw e;
            }
        }

        #endregion
        #region Implementing IDiosposable...
        #region private dispose variable declaration...
        private bool _disposed = false;

        #endregion
        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
