using System.Collections.Generic;

namespace PoolHockeyBLL.Contracts
{
    public interface INewsServices
    {
        IEnumerable<PoolHockeyBLL.ViewModels.NewsFeedVm> GetItems();
    }
}
