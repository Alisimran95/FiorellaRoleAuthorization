using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace FiorellaBackToFrontProject.Models
{
    public class SliderImage
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [NotMapped]
        //[Required]
        public IFormFile Photo { get; set; }

        [NotMapped]
        [Required]
        public IFormFile[] Photos { get; set; }
    }
}
