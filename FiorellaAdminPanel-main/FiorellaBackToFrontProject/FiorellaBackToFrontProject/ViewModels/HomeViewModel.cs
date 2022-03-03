using System.Collections.Generic;
using FiorellaBackToFrontProject.Models;

namespace FiorellaBackToFrontProject.ViewModels
{
    public class HomeViewModel
    {
        public List<SliderImage> SliderImages { get; set; }
        public Slider Slider  { get; set; }
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public List<About> AboutContexts { get; set; }
        public List<AboutIcon> AboutIcons { get; set; }
        public List<FlowerExpert> FlowerExperts { get; set; }
        public FlowerTitle FlowerTitle { get; set; }
        public Subscribe Subscribe  { get; set; }
        public Blog Blog { get; set; }
        public List<BlogItem> BlogItems { get; set; }
        public List<Instagram> Instagrams { get; set; }
    }
}
