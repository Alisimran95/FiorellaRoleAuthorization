using System.ComponentModel.DataAnnotations;

namespace FiorellaBackToFrontProject.Areas.AdminPanel.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required] public string NewPassword { get; set; }

    }
}
