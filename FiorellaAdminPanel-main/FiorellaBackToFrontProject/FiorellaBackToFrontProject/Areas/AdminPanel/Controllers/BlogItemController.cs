using System;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using FiorellaBackToFrontProject.DataAccessLayer;
using FiorellaBackToFrontProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorellaBackToFrontProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BlogItemController : Controller
    {
        private readonly AppDbContext _dbContext;

        public BlogItemController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var blogItems = await  _dbContext.BlogItems.ToListAsync();
            return View(blogItems);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var blogItem = await _dbContext.BlogItems.FindAsync(id);
            if (blogItem == null)
            {
                return NotFound();
            }

            return View(blogItem);
        }

        public  IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogItem blogItem)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existBlogItem =  await _dbContext.BlogItems.AnyAsync(x=>x.BlogTitle.ToLower() == blogItem.BlogTitle.ToLower());
            if (existBlogItem)
            {
                ModelState.AddModelError("BlogTitle","there is blog with this name in database");
                return View();
            }

            //Save blog in database
            await _dbContext.BlogItems.AddAsync(blogItem);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public  IActionResult Update(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(BlogItem blogItem)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existItem  = await _dbContext.BlogItems.FindAsync(blogItem.Id);
            if (existItem == null)
            {
                ModelState.AddModelError("BlogTitle","Item is not found in database");
                return View(existItem);
            };
            var existName = await _dbContext.BlogItems.AnyAsync(x=>x.BlogTitle==blogItem.BlogTitle);

            if (existName)
            {
                ModelState.AddModelError("BlogTitle","The same blog title");
                return View(existItem);
            }

            else
            {
                existItem.BlogTitle = blogItem.BlogTitle;
                existItem.BlogSubtitle = blogItem.BlogSubtitle;
                existItem.Image = blogItem.Image;
                existItem.Date = blogItem.Date;
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var blogItem = await _dbContext.BlogItems.FindAsync(id);
            if (blogItem == null) return Json(new { status = 404 });

            _dbContext.BlogItems.Remove(blogItem);
            await _dbContext.SaveChangesAsync();

            return Json(new { status = 200 });
        }
    }
}
