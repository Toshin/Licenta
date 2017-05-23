using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Licenta_v01.Startup))]
namespace Licenta_v01
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
