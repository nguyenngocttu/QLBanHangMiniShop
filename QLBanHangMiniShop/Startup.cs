using Microsoft.Owin;
using Owin;
using QLBanHangMiniShop;
[assembly: OwinStartupAttribute(typeof(QLBanHangMiniShop.Startup))]
namespace QLBanHangMiniShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            ConfigureAuth(app);
            
        }
    }
}
