using System.Linq;
using System.Security.Cryptography.Xml;
using FiorellaBackToFrontProject.DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorellaBackToFrontProject.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly int _productsCount;

        public ProductsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _productsCount = _dbContext.Products.Count();
        }

        public IActionResult Index()
        {
            ViewBag.ProductCount = _dbContext.Products.Count();

            var products = _dbContext.Products.Take(4).Include(x=>x.Category).ToList();

            return View(products);
        }

        public IActionResult Load(int skip)
        {
            //var products = _dbContext.Products.Include(x => x.Category).Skip(4).Take(4).ToList();
            //return Json(products);
            if (skip >= _productsCount)
            {
                return BadRequest();
            }

            var products = _dbContext.Products.Include(x => x.Category).Skip(skip).Take(4).ToList();
            return PartialView("_ProductPartial", products);
        }

    }
}
