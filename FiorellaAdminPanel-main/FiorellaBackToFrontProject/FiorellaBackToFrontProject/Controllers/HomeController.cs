using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using FiorellaBackToFrontProject.DataAccessLayer;
using FiorellaBackToFrontProject.Models;
using FiorellaBackToFrontProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorellaBackToFrontProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var sliderImages = _dbContext.SliderImages.ToList();
            var slider = _dbContext.Sliders.SingleOrDefault();
            var categories = _dbContext.Categories.ToList();
            var products = _dbContext.Products.Include(x=>x.Category).Take(8).ToList();
            var aboutContexts = _dbContext.AboutContexts.Include(x=>x.AboutIcons).ToList();
            var aboutIcons = _dbContext.AboutIcons.ToList();
            var flowerTitle = _dbContext.FlowerTitles.SingleOrDefault();
            var flowerExperts = _dbContext.FlowerExperts.ToList();
            var subscribes = _dbContext.Subscribes.FirstOrDefault();
            var blog = _dbContext.Blogs.SingleOrDefault();
            var blogItems = _dbContext.BlogItems.ToList();
            var instagrams = _dbContext.Instagrams.ToList();

            return View(new HomeViewModel
                {
                SliderImages = sliderImages,
                Slider = slider,
                Categories = categories,
                Products = products,
                AboutContexts = aboutContexts,
                AboutIcons = aboutIcons,
                FlowerExperts = flowerExperts,
                FlowerTitle = flowerTitle,
                Subscribe = subscribes,
                Blog = blog,
                BlogItems = blogItems,
                Instagrams = instagrams,
                });
        }

        public async  Task<IActionResult> Basket()
        {
            //return Json("ss","sss");
            var basket = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(basket))
            {
                return Content("empty");
            }

            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            var newBasket = new List<BasketViewModel>();
            foreach (var item in basketViewModels)
            {
                var product = await _dbContext.Products.FindAsync(item.Id);
                if (product == null)
                    continue;   
                newBasket.Add(new BasketViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Count = item.Count,
                    Image = product.Image,
                    Amount = item.Amount
                    //Amount =item.Count*product.Price,
                });
            }

            basket = JsonConvert.SerializeObject(newBasket);
            Response.Cookies.Append("basket", basket);

            //return Json(newBasket);
            return View(newBasket);    
        }

        public IActionResult Delete(int id)
        {
            var basket = Request.Cookies["basket"];
            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            var isExist  = basketViewModels.FirstOrDefault(x=>x.Id == id);
            if (isExist == null)
            {
                return NotFound();
            }
            else
            {
                basketViewModels.Remove(isExist);
            }
            var srObject = JsonConvert.SerializeObject(basketViewModels);
            Response.Cookies.Append("basket", srObject);

            return PartialView("_BasketPartial",basketViewModels);
        }

        public IActionResult CountIncrease(int? id)
        {
            var basket = Request.Cookies["basket"];
            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            var isExist = basketViewModels.FirstOrDefault(x=>x.Id==id);
            if (isExist != null)
            {
                isExist.Count++;
            }

            var srObject = JsonConvert.SerializeObject(basketViewModels);
            Response.Cookies.Append("basket",srObject);

            return PartialView("_BasketPartial", basketViewModels);
        }
        public IActionResult CountDecrease(int? id)
        {
            var basket = Request.Cookies["basket"];
            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            var isExist = basketViewModels.FirstOrDefault(x => x.Id == id);
            if (isExist != null && isExist.Count>1)
            {
                isExist.Count--;
            }
            else
            {
                basketViewModels.Remove(isExist);
            }

            var srObject = JsonConvert.SerializeObject(basketViewModels);
            Response.Cookies.Append("basket", srObject);

            return PartialView("_BasketPartial", basketViewModels);
        }


        public async Task<IActionResult> AddToBasket(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return BadRequest();
            };

            List<BasketViewModel> basketViewModels;
            var existBasket = Request.Cookies["basket"];

            if (string.IsNullOrEmpty(existBasket))
            {
                basketViewModels = new List<BasketViewModel>();
            }
            else
            { 
               basketViewModels =  JsonConvert.DeserializeObject<List<BasketViewModel>>(existBasket);
            }

            var existBasketViewModel = basketViewModels.FirstOrDefault(x => x.Id==id);
            if (existBasketViewModel == null)
            {
                existBasketViewModel = new BasketViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Image = product.Image,

                };
                basketViewModels.Add(existBasketViewModel);
            }
            else
            {
                existBasketViewModel.Count++;
            }

            var basket = JsonConvert.SerializeObject(basketViewModels);
            Response.Cookies.Append("basket", basket); ;

            return Json(basketViewModels);
        }
    }
}
