using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Shiny;


namespace POSMovil
{
    public class ShinyAppStartup : Shiny.ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // for general client functionality
            services.UseBleCentral();
            // for client functionality in the background
            //services.UseBleClient<YourBleDelegate>();
            services.UseBlePeripherals();
        }
    }
}
