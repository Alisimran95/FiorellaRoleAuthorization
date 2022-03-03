using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace FiorellaBackToFrontProject.Models
{
    public class FlowerExpert
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string JobPosition { get; set; }
        public string Image { get; set; }

        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
    }
}
