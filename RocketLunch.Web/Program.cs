using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RocketLunch.data;

namespace RocketLunch.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = BuildWebHost(args);
            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                var context = ((LunchContext)services.GetService(typeof(LunchContext)));
                if (context != null)
                {
                    var provider = context.ProviderName;

                    // if not an InMemory database, migrate
                    if (!provider.Contains("InMemory"))
                    {
                        ((LunchContext)services.GetService(typeof(LunchContext))).Migrate();
                    }
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
