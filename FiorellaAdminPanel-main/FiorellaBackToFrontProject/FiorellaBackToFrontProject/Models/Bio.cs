using System.ComponentModel.DataAnnotations;

namespace FiorellaBackToFrontProject.Models
{
    public class Bio
    {

        public int Id { get; set; }

        [Required]
        public string HeaderLogo { get; set; }

        [StringLength(100)]
        public string FacebookUrl { get; set; }

        [StringLength(100)]
        public string LinkedinUrl { get; set; }


    }
}
