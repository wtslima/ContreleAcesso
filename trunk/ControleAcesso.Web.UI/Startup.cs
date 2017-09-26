using Microsoft.Owin;
using Owin;
using Start = ControleAcesso.Web.UI.App_Start.Startup;

[assembly: OwinStartupAttribute(typeof(ControleAcesso.Web.UI.Startup))]
namespace ControleAcesso.Web.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
          var config = new Start();
            config.ConfigureAuth(app);
        }
    }
}
