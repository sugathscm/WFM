using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EML.APP.UI.Startup))]
namespace EML.APP.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
