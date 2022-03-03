using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiorellaBackToFrontProject.Controllers;
using FiorellaBackToFrontProject.DataAccessLayer;
using FiorellaBackToFrontProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorellaBackToFrontProject.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public HeaderViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var TotalCount = 0;
            double TotalAmount = 0;

            var basket = Request.Cookies["basket"];
           
            if (!string.IsNullOrEmpty(basket))
            {
                var products = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
                TotalCount = products.Count;
                foreach (var item in products)
                {
                    TotalAmount += item.Price * item.Count;
                }
            }

            ViewBag.BasketCount = TotalCount;
            ViewBag.TotalAmount = TotalAmount;

            var bio = await _dbContext.Bios.SingleOrDefaultAsync();
            return View(bio);
        }
    }
}
