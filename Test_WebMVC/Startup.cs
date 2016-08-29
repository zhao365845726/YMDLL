using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Test_WebMVC.Startup))]
namespace Test_WebMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
