using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FiorellaBackToFrontProject.Models
{
    public class User:IdentityUser
    {
        [Required]
        public string Fullname { get; set; }

        [Required] public bool IsActive { get; set; } = true;
    }
}
