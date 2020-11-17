using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RocketLunch.data;
using RocketLunch.domain.contracts;
using RocketLunch.domain.services;
using RocketLunch.domain.Services;
using RocketLunch.web.middleware;
using StackExchange.Redis;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Swashbuckle.AspNetCore.Swagger;

namespace RocketLunch.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.ConfigureConfigServerClientOptions(Configuration);

            services.ConfigureCloudFoundryOptions(Configuration);


            ConfigDumper dumper = new ConfigDumper();
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace));
            serviceCollection.AddLogging(builder => builder.AddConsole((opts) =>
            { }));
            serviceCollection.AddLogging(builder => builder.AddDebug());
            var factory = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
            dumper.Run(factory.CreateLogger("config"), Configuration);

            services.AddTransient<IGetLunchOptions, YelpService>(x => new YelpService(Configuration["YELPAPIKEY"], x.GetService<IRestaurantCache>()));
            services.AddTransient<IServeLunch, LunchService>();
            services.AddTransient<IRepository, LunchRepository>();
            services.AddTransient<IManageUsers, UserService>();
            services.AddTransient<IManageTeams, TeamService>();
            services.AddTransient<IManageClaims, ClaimsService>();
            services.AddTransient<IManageUserSessions, UserSessionService>();
            services.AddSingleton<IChaos, RandomService>();


            string vcapServices = Configuration["VCAP_SERVICES"];
            if (vcapServices != null)
            {
                var vcap = JsonConvert.DeserializeObject<vcapservices>(vcapServices);

                Console.WriteLine("using pcf redis");
                Console.WriteLine(JsonConvert.SerializeObject(vcap));
                var redis = vcap.redis[0];
                var redisConfig = new ConfigurationOptions
                {
                    Password = redis.credentials.password
                };
                redisConfig.EndPoints.Add($"{redis.credentials.host}:{redis.credentials.port}");
                services.AddDistributedRedisCache(option =>
                {
                    option.InstanceName = "RocketRedis";
                    option.ConfigurationOptions = redisConfig;
                });
            }
            else
            {
                services.AddDistributedRedisCache(option =>
                {
                    option.Configuration = "localhost:6379";
                    option.InstanceName = "master";
                });
            }


            services.AddTransient<ICache, CacheService>();
            services.AddTransient<IRestaurantCache, RestaurantCache>();


            string connectionString = Configuration["POSTGRESDB"];
            services.AddDbContext<LunchContext>(options => options.UseNpgsql(Configuration["POSTGRESDB"]));


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                // expiration
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;

                // we don't want to redirect the user, instead respond with 401
                options.Events.OnRedirectToLogin = (context) =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            services.AddSwaggerGen(c =>
            {
                // c.DescribeAllEnumsAsStrings();
                c.SwaggerDoc("RocketLunch", new OpenApiInfo { Title = "RocketLunch API", Version = "v1" });
                c.EnableAnnotations();
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {

            app.UseHttpsRedirection();
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/RocketLunch/swagger.json", "RocketLunch");
            });

            app.ConfigureCustomExceptionMiddleware();
            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{*anything}",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
        internal class vcapservices
        {
            [JsonProperty("p-redis")]
            public Redis[] redis { get; set; }

        }

        internal class Redis
        {
            public RedisCreds credentials { get; set; }
        }

        internal class RedisCreds
        {
            public string host { get; set; }
            public string password { get; set; }
            public string port { get; set; }
        }
    }
}