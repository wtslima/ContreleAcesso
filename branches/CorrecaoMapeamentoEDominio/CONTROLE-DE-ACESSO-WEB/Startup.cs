using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CONTROLE_DE_ACESSO_WEB.Startup))]
namespace CONTROLE_DE_ACESSO_WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
