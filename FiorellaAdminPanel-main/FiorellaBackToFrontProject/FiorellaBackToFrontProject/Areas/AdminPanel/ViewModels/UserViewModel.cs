using System.Collections.Generic;
using FiorellaBackToFrontProject.Models;
using Microsoft.AspNetCore.Identity;

namespace FiorellaBackToFrontProject.Areas.AdminPanel.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        //public List<User> Users { get; set; }
        public bool IsActive { get; set; }

    }
}
