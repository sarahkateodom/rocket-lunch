using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RocketLunch.data;
using Steeltoe.Extensions.Configuration.ConfigServer;

namespace RocketLunch.web
{
    using Steeltoe.Extensions.Configuration.CloudFoundry;
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
            string aspNetEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddCloudFoundry()
                .Build();
            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseCloudFoundryHosting()
                .UseSetting("spring:cloud:config:name", "rocketlunch")
                .UseSetting("spring:cloud:config:env", aspNetEnv)
                .AddConfigServer(GetLoggerFactory())
                .UseStartup<Startup>()
                .Build();
        }

        /// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Warning));
            serviceCollection.AddLogging(builder => builder.AddConsole((opts) =>
            { }));
            serviceCollection.AddLogging(builder => builder.AddDebug());
            return serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
        }
    }
}
