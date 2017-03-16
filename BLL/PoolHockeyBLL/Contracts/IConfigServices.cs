using System;

namespace PoolHockeyBLL.Contracts
{
    public interface IConfigServices
    {
        DateTime GetLastUpdate();
        void SetLastUpdate(DateTime d);
    }
}
