using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PoolHockeyBLL.BizEntities;
using PoolHockeyBLL.Contracts;
using PoolHockeyBOL;
using PoolHockeyDAL.UnitOfWork;

namespace PoolHockeyBLL
{
    public class PoolLastYearServices : IPoolLastYearServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICaching _caching;

        public PoolLastYearServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;//new UnitOfWork();
            _caching = new Caching();
        }

        public IEnumerable<PoolLastYearEntity> GetAll()
        {
            var pastPoolInfoCache = (IEnumerable<PoolLastYear>)_caching.GetCachedItem("PastPoolInfoGetAll");

            IEnumerable<PoolLastYear> pastPoolInfo = null;

            if (pastPoolInfoCache == null)
            {
                pastPoolInfo = _unitOfWork.PoolLastYearRepository.GetAll();
                _caching.AddToCache("PastPoolInfoGetAll", pastPoolInfo);
                if (!pastPoolInfo.Any()) return null;
            }
            else
            {
                pastPoolInfo =  pastPoolInfoCache;
            }

            Mapper.CreateMap<PoolLastYear, PoolLastYearEntity>();
            var poolLastYearEntities = Mapper.Map<List<PoolLastYear>, List<PoolLastYearEntity>>((List<PoolLastYear>) pastPoolInfo);

            return poolLastYearEntities;
        }
    }
}
