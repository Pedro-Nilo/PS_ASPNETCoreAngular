using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

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
            services.AddIdentity<StoreUser, IdentityRole>(configuration =>
            {
                configuration.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<DutchContext>();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(configuration => {
                    configuration.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = _configurationFile["Tokens:Issuer"],
                        ValidAudience = _configurationFile["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationFile["Tokens:Key"]))
                    };
                });

            services.AddDbContext<DutchContext>(configuration => 
            {
                configuration.UseNpgsql(_configurationFile.GetConnectionString("DutchConnectionString"));
            });

            services.AddTransient<DutchSeeder>();
            services.AddTransient<IMailService, NullMailService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IDutchRepository, DutchRepository>();
            
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddMvc()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
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

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(configuration => {
                configuration.MapRazorPages();
                configuration.MapControllerRoute("Fallback",
                    "{controller}/{action}/{id?}",
                    new { controller = "App", action = "Index" });
            });
        }
    }
}
