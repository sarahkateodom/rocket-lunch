using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RocketLunch.data;
using RocketLunch.domain.contracts;
using RocketLunch.domain.services;
using RocketLunch.web.middleware;
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
            services.AddTransient<IGetLunchOptions, YelpService>(x => new YelpService(Configuration["YELPAPIKEY"], x.GetService<IRestaurantCache>()));
            services.AddTransient<IServeLunch, LunchService>();
            services.AddTransient<IRepository, LunchRepository>();
            services.AddTransient<IManageUsers, UserService>();
            services.AddTransient<IManageUserSessions, UserSessionService>();
            services.AddSingleton<IChaos, RandomService>();
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = "localhost:6379";
                option.InstanceName = "master";
            });
            services.AddTransient<ICache, CacheService>();
            services.AddTransient<IRestaurantCache, RestaurantCache>();


            string connectionString = Configuration["POSTGRESDB"];
            services.AddDbContext<LunchContext>(options => options.UseMySQL(Configuration["POSTGRESDB"]));


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
                c.SwaggerDoc("RocketLunch", new Info { Title = "RocketLunch API", Version = "v1" });
                c.EnableAnnotations();
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
    }
}