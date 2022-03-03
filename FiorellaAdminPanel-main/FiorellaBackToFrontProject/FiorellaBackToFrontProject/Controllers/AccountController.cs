using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FiorellaBackToFrontProject.Areas.AdminPanel.ViewModels;
using FiorellaBackToFrontProject.Data;
using FiorellaBackToFrontProject.Models;
using FiorellaBackToFrontProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ChangePasswordViewModel = FiorellaBackToFrontProject.ViewModels.ChangePasswordViewModel;

namespace FiorellaBackToFrontProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existUser = await _userManager.FindByNameAsync(registerViewModel.Username);
            if (existUser != null)
            {
                ModelState.AddModelError("Username", "There is user with this name ");
                return View();
            }

            var user = new User()
            {
                Fullname = registerViewModel.Fullname,
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email,

            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();

            }

            await _userManager.AddToRoleAsync(user, RoleConstants.UserRole);
            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel,UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //Admin panel deactive edibse , user sayta daxil ola bilmez.

            if (userViewModel.IsActive == true)
            {
                var existUser = await _userManager.FindByNameAsync(loginViewModel.Username);
                if (existUser == null)
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(existUser, loginViewModel.Password,
                    loginViewModel.RememberMe, false);

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                    return View();
                }

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                    return View();
                }

            }
            else
            {
                return BadRequest();
            }


            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Email", "Invalid Credentials");
                return View();

            }

            var existEmail = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email );
            if (existEmail == null)
            {
                ModelState.AddModelError("Email", "Email not found");
                return View();
            }

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(existEmail);
            string link = Url.Action(nameof(ChangePassword), "Account",
                new {email = resetPasswordViewModel.Email, token}, Request.Scheme, Request.Host.ToString());


            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("codep320@gmail.com", "IdrisAlisimran");
            msg.To.Add(resetPasswordViewModel.Email);
            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/template/Verify.html"))
            {
                body = await reader.ReadToEndAsync();
            }

            msg.Body = body.Replace("{{link}}", link);
            msg.Subject = "Verify";
            msg.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("codep320@gmail.com", "codeacademyp320");
            smtp.Send(msg);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangePassword(ResetPasswordViewModel resetPasswordViewModel,
            ChangePasswordViewModel changePasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "invalid credentials");
                return View();
            }

            var existUser = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (existUser == null)
            {
                ModelState.AddModelError("", "User not found");
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(existUser);

            await _userManager.ResetPasswordAsync(existUser, token, changePasswordViewModel.Password);
            await _signInManager.RefreshSignInAsync(existUser);

            return RedirectToAction("Index", "Home");
        }


        public IActionResult ChangeUserPassword()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangeUserPassword(ChangeUserInfoViewModel changeUserInfoViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Invalid Credentials");
                return View();
            }

            var existUser = await _userManager.GetUserAsync(User);

            var password =  await _userManager.ChangePasswordAsync(existUser,changeUserInfoViewModel.OldPassword,changeUserInfoViewModel.NewPassword);
           
            if (!password.Succeeded)
            {
                ModelState.AddModelError("OldPassword","Old password isn't true");
                return View();  
            }
            await _signInManager.RefreshSignInAsync(existUser);

            return RedirectToAction("Index", "Home");

        }
    }
}
