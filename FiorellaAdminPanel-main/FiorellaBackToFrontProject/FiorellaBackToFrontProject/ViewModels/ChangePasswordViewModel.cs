using System.ComponentModel.DataAnnotations;

namespace FiorellaBackToFrontProject.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}
