using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LendingSystem.Startup))]
namespace LendingSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
