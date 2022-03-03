using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace FiorellaBackToFrontProject.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
