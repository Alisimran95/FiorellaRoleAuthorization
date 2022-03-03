using System;
using System.IO;
using System.Threading.Tasks;
using FiorellaBackToFrontProject.DataAccessLayer;
using FiorellaBackToFrontProject.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace FiorellaBackToFrontProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class FlowerExpertController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public FlowerExpertController(AppDbContext dbContext,IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var flowerExperts =await _dbContext.FlowerExperts.ToListAsync();
            return View(flowerExperts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlowerExpert flowerExpert)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!flowerExpert.Photo.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Photo","Please upload image format");
                return View();
            }

            if (flowerExpert.Photo.Length > 1024*1000)
            {
                ModelState.AddModelError("Photo","Max upload size must be smaller than 1 MB");
                return View();
            }

            var webRootPath = _environment.WebRootPath;
            var fileName = $"{Guid.NewGuid()}-{flowerExpert.Photo.FileName}";
            var path = Path.Combine(webRootPath, "img", fileName);

            var fileStream = new FileStream(path,FileMode.CreateNew);
            await flowerExpert.Photo.CopyToAsync(fileStream);
            flowerExpert.Image = fileName;

            await _dbContext.FlowerExperts.AddAsync(flowerExpert);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }

            var flowerExpert = await _dbContext.FlowerExperts.FirstOrDefaultAsync(x=>x.Id==id);
            if (flowerExpert == null)
            {
                return NotFound();
            }
            return View(flowerExpert);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, FlowerExpert flowerExpert)
        {
            if (id==null)
            {
                return NotFound();
            }

            if (id != flowerExpert.Id)
            {
                return BadRequest();
            }

            var existExpert = await _dbContext.FlowerExperts.FindAsync(id);

            if (existExpert == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(existExpert);
            }
            existExpert.Name = flowerExpert.Name;
            existExpert.JobPosition = flowerExpert.JobPosition;
            existExpert.Image = flowerExpert.Photo.FileName;
            existExpert.Photo = flowerExpert.Photo;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }

            var flowerExpert = await _dbContext.FlowerExperts.FindAsync(id);
            if (flowerExpert ==null)
            {
                return NotFound();
            }

            return View(flowerExpert);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var isDeleted = await _dbContext.FlowerExperts.FindAsync(id);
            if (isDeleted == null)
            {
                return NotFound();
            }

            _dbContext.FlowerExperts.Remove(isDeleted);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
