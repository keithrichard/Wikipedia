using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Wikipedia.Startup))]

namespace Wikipedia
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }        
    }
}
