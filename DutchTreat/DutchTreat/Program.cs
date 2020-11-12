using DutchTreat.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DutchTreat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            SeedDatabase(host);

            host.Run();
        }

        private static void SeedDatabase(IHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();

            using var scope = scopeFactory.CreateScope();

            var seeder = scope.ServiceProvider.GetService<DutchSeeder>();

            seeder.SeedAsync().Wait();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SetupConfiguration)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void SetupConfiguration(HostBuilderContext hostBuilderContext, IConfigurationBuilder builder)
        {
            builder.Sources.Clear();

            builder.AddJsonFile("config.json", false, true)
                   .AddXmlFile("config.xml", true)
                   .AddEnvironmentVariables();
        }
    }
}
