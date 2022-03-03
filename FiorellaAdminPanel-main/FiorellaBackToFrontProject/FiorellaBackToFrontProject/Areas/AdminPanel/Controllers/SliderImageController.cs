using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FiorellaBackToFrontProject.Areas.AdminPanel.Data;
using FiorellaBackToFrontProject.DataAccessLayer;
using FiorellaBackToFrontProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorellaBackToFrontProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SliderImageController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;


        public SliderImageController(AppDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var sliderImages = await _dbContext.SliderImages.ToListAsync();

            return View(sliderImages);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(SliderImage sliderImage)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    if (!sliderImage.Photo.IsImage()) 
        //    {
        //        ModelState.AddModelError("Photo","Uploaded file must be image type..");
        //        return View();  
        //    }
        //    if (!sliderImage.Photo.IsAllowedSize(1))
        //    {
        //        ModelState.AddModelError("Photo", "Memory of file must be smaller than 1 MB");
        //        return View();
        //    }


        //    var fileName = await sliderImage.Photo.GenerateFile(Constants.ImageFolderPath);

        //    sliderImage.Name = fileName;
        //    await _dbContext.SliderImages.AddAsync(sliderImage);
        //    await _dbContext.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}
        public async Task<IActionResult> Create(SliderImage sliderImage)
        {
            if (!ModelState.IsValid)
                return View();
            var sliderCountInDatabase = _dbContext.SliderImages.Count();
            var sliderImagesCount = sliderImage.Photos.Length;
            var totalSliderCount = sliderImagesCount + sliderCountInDatabase;

            if (totalSliderCount > 5)
            {
                ModelState.AddModelError("Photos", "Slider must be 5 or less");
                return View();
            }

            foreach (var photo in sliderImage.Photos)
            {
                if (!photo.IsImage())
                {
                    ModelState.AddModelError("Photos", $"{photo.FileName} - must be image");
                    return View();
                }

                if (!photo.IsAllowedSize(1))
                {
                    ModelState.AddModelError("Photos", $"{photo.FileName} - file size must be smaller than 1mb");
                    return View();
                }

                var fileName = await photo.GenerateFile(Constants.ImageFolderPath);

                var newSliderImage = new SliderImage { Name = fileName };

                await _dbContext.SliderImages.AddAsync(newSliderImage);
                await _dbContext.SaveChangesAsync();
            }

            //return Json(sliderImagesCount);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();

            var sliderImage = await _dbContext.SliderImages.FirstOrDefaultAsync(x => x.Id == id);
            if (sliderImage == null)
                return NotFound();

            return View(sliderImage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SliderImage sliderImage)
        {
            if (id == null)
                return NotFound();

            if (id != sliderImage.Id)
                return BadRequest();

            var existSliderImage = await _dbContext.SliderImages.FindAsync(id);
            if (existSliderImage == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(existSliderImage);

            if (!sliderImage.Photo.IsImage())
            {
                ModelState.AddModelError("Photos", "Please upload image format");
                return View(existSliderImage);
            }

            if (!sliderImage.Photo.IsAllowedSize(1))
            {
                ModelState.AddModelError("Photos", "Max upload size must be smaller than 1 MB");
                return View(existSliderImage);
            }

            var path = Path.Combine(Constants.ImageFolderPath, existSliderImage.Name);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
                
            var fileName = await sliderImage.Photo.GenerateFile(Constants.ImageFolderPath);
            existSliderImage.Name = fileName;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var sliderImage = await _dbContext.SliderImages.FirstOrDefaultAsync(x => x.Id == id);
            if (sliderImage == null)
                return NotFound();

            return View(sliderImage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
                return NotFound();

            var existSliderImage = await _dbContext.SliderImages.FindAsync(id);
            if (existSliderImage == null)
                return NotFound();

            var path = Path.Combine(Constants.ImageFolderPath, existSliderImage.Name);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _dbContext.SliderImages.Remove(existSliderImage);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }

            var existSliderPhoto = await _dbContext.SliderImages.FindAsync(id);
            if (existSliderPhoto.Id != id)
            {
                return BadRequest();
            }

            return View(existSliderPhoto);
        }
    }
}
