using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketLunch.data;
using RocketLunch.domain.contracts;
using RocketLunch.domain.services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            services.AddTransient<IGetLunchOptions, YelpService>(x => new YelpService(Configuration["YELPAPIKEY"]));
            services.AddTransient<IServeLunch, LunchService>();
            services.AddTransient<IRepository, LunchRepository>();
            services.AddTransient<IManageUsers, UserService>();
            services.AddTransient<IManageUserSessions, UserSessionService>();
            services.AddSingleton<IChaos, RandomService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            string connectionString = Configuration["POSTGRESDB"];
            services.AddDbContext<LunchContext>(options => options.UseNpgsql(Configuration["POSTGRESDB"]));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            // app.UseHttpsRedirection();
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
