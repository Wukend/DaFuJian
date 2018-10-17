using System;
using System.Threading.Tasks;
using System.Web.Http;
using Dafujian.Authentication.Handlers;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;

[assembly: OwinStartup(typeof(Dafujian.Authentication.Startup))]

namespace Dafujian.Authentication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            app.Use((context, next) =>
            {
                JwtAuthHandler.OnAuthenticateRequest(context); //the new method
                return next.Invoke();
            });
            app.UseStageMarker(PipelineStage.Authenticate);
            WebApiConfig.Register(config);//Remove or comment the config.MessageHandlers.Add(new JwtAuthHandler()) section it would not be triggered on execution.

            app.UseWebApi(config);
        }
    }
}
