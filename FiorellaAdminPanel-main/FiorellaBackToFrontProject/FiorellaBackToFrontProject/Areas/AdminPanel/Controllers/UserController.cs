#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FiorellaBackToFrontProject.Areas.AdminPanel.ViewModels;
using FiorellaBackToFrontProject.Data;
using FiorellaBackToFrontProject.DataAccessLayer;
using FiorellaBackToFrontProject.Models;
using FiorellaBackToFrontProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChangePasswordViewModel = FiorellaBackToFrontProject.Areas.AdminPanel.ViewModels.ChangePasswordViewModel;

namespace FiorellaBackToFrontProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        //private readonly SignInManager<> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        
        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _appDbContext.Users.ToListAsync();
            List<UserViewModel> userViewModels = new List<UserViewModel>();
            foreach (var user in users)
            {
                UserViewModel userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                    IsActive = user.IsActive,
                };
                userViewModels.Add(userViewModel);
            }
            return View(userViewModels);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id,ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","invalid credentials");
                return View();
            }

            var existUser = await _userManager.FindByIdAsync(id);
            if (existUser == null)
            {
                return BadRequest();
            }

            var checkPassword = await _userManager.CheckPasswordAsync(existUser,model.OldPassword);
            if (checkPassword == false)
            {
                ModelState.AddModelError("OldPassword","User password is incorrect");
            }

            var result =  await _userManager.ChangePasswordAsync(existUser,model.OldPassword,model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("","invalid credentials");
                    return View();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Activate(string id,UserViewModel userViewModel)
        {
            if (id == null)
            {
                return NotFound();
            }
            var existUser = await _userManager.FindByIdAsync(id);
            if (existUser == null)
            {
                return NotFound();
            }

            existUser.IsActive = existUser.IsActive != true;
            await _userManager.UpdateAsync(existUser);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> AddRole(string? id)
        {
            if (id==null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(id);

            if (user==null)
            {
                return NotFound();
            }

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (role==null)
            {
                return NotFound();
            }
            //await  _userManager.GetUsersInRoleAsync(role);
            //ViewData["role"] = role;
            ViewBag.Role =role;
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(string? id, string changedRole)
        {
            if (id==null)
            {
                return BadRequest();
            }
            var existUser = await _userManager.FindByIdAsync(id);
           
            if (existUser == null)
            {
                return NotFound();
            }

            var firstRole = (await _userManager.GetRolesAsync(existUser)).FirstOrDefault();

            if (firstRole==null)
            {
                return NotFound();
            }

            if (firstRole != changedRole)
            {
                var removedRole = await _userManager.RemoveFromRoleAsync(existUser, firstRole);

                if (!removedRole.Succeeded)
                {
                    ModelState.AddModelError("", "Something is wrong ");
                }

                var changedResult = await _userManager.AddToRoleAsync(existUser, changedRole);

                if (!changedResult.Succeeded)
                {
                    ModelState.AddModelError("", "Something is wrong");
                }

                

                await _userManager.UpdateAsync(existUser);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
