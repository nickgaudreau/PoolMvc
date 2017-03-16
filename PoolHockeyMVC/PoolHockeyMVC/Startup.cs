using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PoolHockeyMVC.Startup))]
namespace PoolHockeyMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
