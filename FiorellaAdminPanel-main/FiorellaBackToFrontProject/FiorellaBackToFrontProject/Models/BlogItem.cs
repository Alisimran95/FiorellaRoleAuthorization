using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace FiorellaBackToFrontProject.Models
{
    public class BlogItem
    {
        public int Id { get; set; }

        [Required ,MaxLength(200)]
        public string Image { get; set; }

        [Required]
        public string BlogTitle { get; set; }

        [Required]
        public string BlogSubtitle { get; set; }

        public DateTime Date  { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return Date.ToString("MM/dd/yyyy").Replace("/", ".");

        }
    }
}
