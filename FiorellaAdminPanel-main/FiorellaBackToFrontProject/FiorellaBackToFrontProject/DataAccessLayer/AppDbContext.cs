using System.Reflection.Metadata.Ecma335;
using FiorellaBackToFrontProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FiorellaBackToFrontProject.DataAccessLayer
{
    public class AppDbContext:IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderImage> SliderImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<About> AboutContexts { get; set; }
        public DbSet<AboutIcon> AboutIcons { get; set; }
        public DbSet<FlowerTitle> FlowerTitles { get; set; }
        public DbSet<FlowerExpert> FlowerExperts { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogItem> BlogItems { get; set; }
        public DbSet <Instagram> Instagrams { get; set; }
        public DbSet<Bio> Bios { get; set; }
    }
}
