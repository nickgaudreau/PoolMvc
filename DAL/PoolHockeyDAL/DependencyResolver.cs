using System.ComponentModel.Composition;
using PoolHockeyDAL.UnitOfWork;
using Resolver;

namespace PoolHockeyDAL
{
    [Export(typeof(IComponent))]
    public class DependencyResolver : IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            registerComponent.RegisterType<IUnitOfWork, UnitOfWork.UnitOfWork>();
        }
    }
}
