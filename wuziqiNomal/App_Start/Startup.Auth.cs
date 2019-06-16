using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.AspNet;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace wuziqiNomal
{
    public partial class Startup
    {
        void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Chezugou",
                LoginPath = new PathString(ConfigurationManager.AppSettings["LoginPath"]),
                CookieDomain = ConfigurationManager.AppSettings["CookieDomain"]
            });
        }
    }
}
