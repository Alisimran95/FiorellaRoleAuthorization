using System.ComponentModel.DataAnnotations;

namespace FiorellaBackToFrontProject.ViewModels
{
    public class ChangeUserInfoViewModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
