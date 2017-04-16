using System;
using PoolHockeyBLL.Contracts;
using PoolHockeyDAL.UnitOfWork;

namespace PoolHockeyBLL
{
    public class ConfigServices : IConfigServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICaching _caching;
        private  const int SmarterAspServerTimeDifference = 3; // PST

        
        public ConfigServices()// no DI: UnitOfWork unitOfWork)
        {
            _unitOfWork = new UnitOfWork();  // can';t usae di for this one wince we call this utlity from the Main Layout whci has no cosntructor
            _caching = new Caching();
        }

        
        public DateTime GetLastUpdate()
        {
            var lastUpdateCache = _caching.GetCachedItem("LastUpdate");

            DateTime lastUpdate;
            if (lastUpdateCache == null)
            {
                lastUpdate = _unitOfWork.GetLastUpdate();
                _caching.AddToCache("LastUpdate", lastUpdate);
            }
            else
            {
                lastUpdate = (DateTime)lastUpdateCache;
            }
            
            return lastUpdate;
        }

        public void SetLastUpdate(DateTime dateTime)
        {
            try
            {
                var dateTimeFixed = dateTime.AddHours(SmarterAspServerTimeDifference);
                _unitOfWork.SetLastUpdate(dateTimeFixed);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
