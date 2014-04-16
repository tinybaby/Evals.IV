using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Evals.IV.Store.Startup))]
namespace Evals.IV.Store
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
