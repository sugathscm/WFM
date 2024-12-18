using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WFM.UI.DF.Startup))]
namespace WFM.UI.DF
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
