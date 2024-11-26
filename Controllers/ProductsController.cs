﻿using BakeryStoreMVC.Models;
using BakeryStoreMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BakeryStoreMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
		private readonly IWebHostEnvironment environment;

		public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
			this.environment = environment;
		}

        public IActionResult Index()
        {
            var products = context.Product.OrderByDescending(p => p.Id).ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View(); 
        }


        [HttpPost]
		public IActionResult Create(ProductDto productDto)
		{
            if(productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The Image file is required!");
            }
			if (!ModelState.IsValid)
			{
                return View(productDto);
			}


			// save the image file
			string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
			newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

			string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
			using (var stream = System.IO.File.Create(imageFullPath))
			{
				productDto.ImageFile.CopyTo(stream);
			}

			// save the new product in the database
			Product product = new Product()
			{
				Name = productDto.Name,
				Brand = productDto.Brand,
				Category = productDto.Category,
				Price = productDto.Price,
				Description = productDto.Description,
				ImageFileName = newFileName,
				CreatedAt = DateTime.Now,
			};


			context.Product.Add(product);
			context.SaveChanges();

            return RedirectToAction("index", "Products");
		}

		public IActionResult Edit(int id)
		{
			var product = context.Product.Find(id);
			if (product == null)
			{
				return RedirectToAction("Index", "Products");
			}

			//Create prodcutDto from product
			var productDto = new ProductDto()
			{
				Name = product.Name,
				Brand = product.Brand,
				Category = product.Category,
				Price = product.Price,
				Description = product.Description,

			};


			ViewData["ProductId"] = product.Id;
			ViewData["ImageFileName"] = product.ImageFileName;
			ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
			return View(productDto);
		}
	}
}
