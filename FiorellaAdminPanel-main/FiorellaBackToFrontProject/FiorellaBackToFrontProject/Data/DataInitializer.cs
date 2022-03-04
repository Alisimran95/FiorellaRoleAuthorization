using System.Collections.Generic;
using System.Threading.Tasks;
using FiorellaBackToFrontProject.DataAccessLayer;
using FiorellaBackToFrontProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FiorellaBackToFrontProject.Data
{
    public class DataInitializer
    {
        private readonly AppDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public DataInitializer(AppDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedDataAsync()
        {
            await _dbContext.Database.MigrateAsync();

            var roles = new List<string>
            {
                RoleConstants.AdminRole,
                RoleConstants.ModeratorRole,
                RoleConstants.UserRole
            };

            foreach (var role in roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                    continue;

                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            var user = new User
            {
                Fullname = "Admin Ali",
                UserName = "Admin",
                Email = "admin@code.az"
            };
            var moderator = new User
            {
                Fullname = "Idris Jafarzade",
                UserName = "Moderator",
                Email = "idris@mail.ru"
            };
            if (await _userManager.FindByNameAsync(user.UserName) != null && await _userManager.FindByNameAsync(moderator.UserName)!=null)
                return;
            
            await _userManager.CreateAsync(user, "1234@Admin");
            await _userManager.CreateAsync(moderator,"1234@Idris");
            await _userManager.AddToRoleAsync(moderator,RoleConstants.ModeratorRole);
            await _userManager.AddToRoleAsync(user, RoleConstants.AdminRole);
        }
    }
}
