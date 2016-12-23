using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LernEnglishWords.Startup))]
namespace LernEnglishWords
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
