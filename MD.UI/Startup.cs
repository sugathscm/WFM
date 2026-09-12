using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MD.UI.Startup))]
namespace MD.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
