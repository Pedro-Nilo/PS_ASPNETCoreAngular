using DutchTreat.Data;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DutchTreat
{
    public class Startup
    {
        private readonly IConfiguration _configurationFile;

        public Startup(IConfiguration configurationFile)
        {
            _configurationFile = configurationFile;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DutchContext>(configuration => 
            {
                configuration.UseNpgsql(_configurationFile.GetConnectionString("DutchConnectionString"));
            });

            services.AddTransient<DutchSeeder>();
            services.AddTransient<IMailService, NullMailService>();

            services.AddScoped<IDutchRepository, DutchRepository>();
            
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStaticFiles();
            app.UseNodeModules();

            app.UseRouting();
            
            app.UseEndpoints(configuration => {
                configuration.MapRazorPages();
                configuration.MapControllerRoute("Fallback",
                    "{controller}/{action}/{id?}",
                    new { controller = "App", action = "Index" });
            });
        }
    }
}
