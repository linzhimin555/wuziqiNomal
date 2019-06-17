using System;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet;

[assembly: OwinStartup(typeof(wuziqiNomal.Startup))]
namespace wuziqiNomal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}