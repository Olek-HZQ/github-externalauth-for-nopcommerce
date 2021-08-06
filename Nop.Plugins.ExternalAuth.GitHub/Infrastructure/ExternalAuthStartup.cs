using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;

namespace Nop.Plugin.ExternalAuth.GitHub.Infrastructure
{
    public class ExternalAuthStartup: INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSameSiteCookiePolicy(); // cookie policy to deal with temporary browser incompatibilities
        }

        public void Configure(IApplicationBuilder application)
        {
            application.UseCookiePolicy(); // added this, Before UseAuthentication or anything else that writes cookies.
        }

        public int Order => 15;
    }
}
