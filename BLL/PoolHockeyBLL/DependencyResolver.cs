
using System.ComponentModel.Composition;
using PoolHockeyBLL.Contracts;
using Resolver;

namespace PoolHockeyBLL
{
    [Export(typeof(IComponent))]
    public class DependencyResolver : IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            // if more services just dup this line with <IOtherServ, OtherServ> ?
            registerComponent.RegisterType<IPlayerInfoServices, PlayerInfoServices>();
            registerComponent.RegisterType<IUserInfoServices, UserInfoServices>();
            registerComponent.RegisterType<IPoolLastYearServices, PoolLastYearServices>();
            registerComponent.RegisterType<IPastPlayerInfoServices, PastPlayerInfoServices>();
            registerComponent.RegisterType<IConfigServices, ConfigServices>();
            registerComponent.RegisterType<ITeamScheduleServices, TeamScheduleServices>();
            registerComponent.RegisterType<INewsServices, NewsServices>();
        }
    }
}
