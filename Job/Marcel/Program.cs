using Marcel.Access;
using Marcel.Browser;
using Marcel.DbModels.Model;
using Marcel.Pure.Co.UK;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Marcel
{
    internal class Program
    {
        private static IConfiguration Configuration { get; set; }

        private static void Main(string[] args)
        {
            // shouldn't use console application
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string appSettings = env?.ToLower() == "production" ? "appsettings.production.json" : "appsettings.json";
            Configuration = new ConfigurationBuilder()
                   .AddJsonFile(appSettings, optional: true, reloadOnChange: true)
                   .Build();
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Information);
                })
                .AddSingleton(Configuration)
                .AddSingleton<IWebDriverFactory, WebDriverFactory>()
                .AddTransient<IDishPageService, DishPageService>()
                .AddTransient<IMenuPageService, MenuPageService>()
                .AddTransient<INavigationService, NavigationService>()
                .AddTransient<IDishAccess, DishAccess>()
                .AddDbContext<MyDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyDatabase")))
                .BuildServiceProvider();

            var context = serviceProvider.GetService<MyDbContext>();
            // migrate any database changes on startup (includes initial db creation)
            context.Database.Migrate();

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");

            //do the actual work here
            var page = serviceProvider.GetService<INavigationService>();
            var dataAccess = serviceProvider.GetService<IDishAccess>();
            var results = page.Sniff("https://www.pure.co.uk/menus/breakfast/");

            foreach (var result in results)
            {
                if (dataAccess.Find(result.Url) != null)
                {
                    dataAccess.Update(result);
                }
                else
                {
                    dataAccess.Insert(result);
                }
            }

            logger.LogDebug("All done!");
        }
    }
}