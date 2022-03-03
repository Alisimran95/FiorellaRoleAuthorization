using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiorellaBackToFrontProject.Data;
using FiorellaBackToFrontProject.DataAccessLayer;
using FiorellaBackToFrontProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FiorellaBackToFrontProject
{
    public class Program
    {
        public  static async Task Main(string[] args)
        {
           var host =  CreateHostBuilder(args).Build();

           using (var scope = host.Services.CreateScope())
           {
               var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
               var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
               var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

               var dataInitializer = new DataInitializer(appDbContext,roleManager,userManager);
               await dataInitializer.SeedDataAsync();

           }

           await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
