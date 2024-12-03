using BakeryStoreMVC.Models;
using BakeryStoreMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BakeryStoreMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
		private readonly IWebHostEnvironment environment;
		private readonly int pageSize = 5;

		public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
			this.environment = environment;
		}

        public IActionResult Index(int pageIndex, string? search)
        {
			IQueryable<Product> query = context.Product;

			query = query.OrderByDescending(p => p.Id);
			
			//search fucntionality
			if(search != null)
			{
				query = query.Where(p => p.Name.Contains(search) || p.Brand.Contains(search));
			}

			//pagination functionality
			if(pageIndex < 1)
			{
				pageIndex = 1;
			} 

			decimal count = query.Count();
			int totalPages = (int)Math.Ceiling(count / pageSize);
			query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var products = query.ToList();

			ViewData["PageIndex"] = pageIndex;
			ViewData["TotalPages"] = totalPages;

			ViewData["Search"] = search ?? "";



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

		[HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
		{
			var product = context.Product.Find(id);

			if(product == null)
			{
				return RedirectToAction("index", "Products");
			}

			if (!ModelState.IsValid)
			{
				ViewData["ProductID"] = product.Id;
				ViewData["ImageFileName"] = product.ImageFileName;
				ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

				return View(productDto);
			}

			//update the image file if we have a new image file
			string newFileName = product.ImageFileName;
			if(productDto.ImageFile != null)
			{
				newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
				newFileName += Path.GetExtension(productDto.ImageFile.FileName);

				string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
				using (var stream = System.IO.File.Create(imageFullPath))
				{
					productDto.ImageFile.CopyTo(stream);
				}
			}

			//delete the old image
			string oldImageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
			System.IO.File.Delete(oldImageFullPath);

			//update the product in the database
			product.Name = productDto.Name;
			product.Brand = productDto.Brand;
			product.Category = productDto.Category;
			product.Price = productDto.Price;
			product.Description = productDto.Description;
			product.ImageFileName = newFileName;

			context.SaveChanges();
			return RedirectToAction("index", "Products");
		}

		public IActionResult Delete(int id)
		{
            var product = context.Product.Find(id);

            if (product == null)
            {
                return RedirectToAction("index", "Products");
            }

			string imageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
			System.IO.File.Delete(imageFullPath);

			context.Product.Remove(product);
			context.SaveChanges(true);
            return RedirectToAction("index", "Products");
        }

    }
}
