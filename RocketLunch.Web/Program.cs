using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RocketLunch.data;
using Steeltoe.Extensions.Configuration.ConfigServer;

namespace RocketLunch.web
{
    using Microsoft.Extensions.Hosting;
    using Steeltoe.Common.Hosting;
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args).Build();
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

        public static IHostBuilder BuildWebHost(string[] args)
        {
            string aspNetEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            // var config = new ConfigurationBuilder()
            //     .AddCommandLine(args)
            //     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //     .SetBasePath(Directory.GetCurrentDirectory())
            //     .AddEnvironmentVariables()
            //     .AddCloudFoundry()
            //     .Build();
            // return WebHost.CreateDefaultBuilder(args)
            //     .UseConfiguration(config)
            //     .UseCloudFoundryHosting()
            //     .UseSetting("spring:cloud:config:name", "rocketlunch")
            //     .UseSetting("spring:cloud:config:env", aspNetEnv)
            //     .AddConfigServer(GetLoggerFactory())
            //     .UseStartup<Startup>()
            //     .Build();
            return Host.CreateDefaultBuilder(args)
					.UseContentRoot(Directory.GetCurrentDirectory())
					.ConfigureAppConfiguration((hostingContext, config) =>
					{
						var env = hostingContext.HostingEnvironment;

						config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
						if (aspNetEnv is "Localhost" || aspNetEnv is "Integration" || aspNetEnv is "Mock")
							config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
					})
					.UseCloudHosting()
					.ConfigureLogging((hostingContext, logging) =>
					{
						logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
						logging.AddConsole();
						logging.AddDebug();
						logging.AddEventSourceLogger();
					})
					.ConfigureWebHostDefaults(webBuilder =>
						webBuilder
						.UseSetting("spring:cloud:config:name", "rocketlunch")
						.UseSetting("spring:cloud:config:env", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
						.UseStartup<Startup>()
						.UseKestrel(options => options.AllowSynchronousIO = true) // see issue https://github.com/dotnet/aspnetcore/issues/8302
					)
					.AddConfigServer(GetLoggerFactory());
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
