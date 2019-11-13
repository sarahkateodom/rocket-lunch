using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RocketLunch.data;
using RocketLunch.domain.contracts;
using RocketLunch.domain.services;

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
            services.AddDbContext<LunchContext>(options => options.UseNpgsql(Configuration["POSTGRESDB"]));

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot";
            });

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

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

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });


            app.UseSpa(spa => { });
        }
    }
}