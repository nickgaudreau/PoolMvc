using System.Collections.Generic;
using PoolHockeyBLL.BizEntities;

namespace PoolHockeyBLL.Contracts
{
    public interface IPoolLastYearServices
    {
        IEnumerable<PoolLastYearEntity> GetAll();
    }
}
