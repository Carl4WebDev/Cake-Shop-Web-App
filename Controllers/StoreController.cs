using BakeryStoreMVC.Models;
using BakeryStoreMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BakeryStoreMVC.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly int pageSize = 8;
        public StoreController(ApplicationDbContext context) {
            this.context = context;
        } 
        public IActionResult Index(int pageIndex)
        {
            IQueryable<Product> query = context.Product;

            query = query.OrderByDescending(p=> p.Id);

            if (pageIndex < 1) {
                pageIndex = 1;
            }

            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count/ pageSize);
            query = query.Skip((pageIndex -1) * pageSize).Take(pageSize);


            var products = query.ToList(); 
            ViewBag.Products = products;
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = totalPages;
            return View();
        }
    }
}
