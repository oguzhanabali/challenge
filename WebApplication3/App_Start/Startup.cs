using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebApplication3;
using WebApplication3.Auth;
using WebApplication3.Models;

[assembly: OwinStartup(typeof(WebApplication3.Startup))]
namespace WebApplication3
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            //TODO:Cors'u etkinleştir.
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.CreatePerOwinContext<OwinAuthDbContext>(() => new OwinAuthDbContext());
            app.CreatePerOwinContext<UserManager<IdentityUser>>(CreateManager);
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new MyAuthorizationProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                AllowInsecureHttp = true
               
            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private static UserManager<IdentityUser> CreateManager(IdentityFactoryOptions<UserManager<IdentityUser>> options, IOwinContext context)
        {
            var userStore = new UserStore<IdentityUser>(context.Get<OwinAuthDbContext>());
            var owinManager = new UserManager<IdentityUser>(userStore);
            return owinManager;
        }

    }
}
