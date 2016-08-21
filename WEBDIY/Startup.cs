using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WEBDIY.Startup))]
namespace WEBDIY
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
