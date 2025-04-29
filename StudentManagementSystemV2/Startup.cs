using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentManagementSystemV2.Startup))]
namespace StudentManagementSystemV2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
