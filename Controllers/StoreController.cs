using BakeryStoreMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BakeryStoreMVC.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext context;
        public StoreController(ApplicationDbContext context) {
            this.context = context;
        }
        public IActionResult Index()
        {
            var products = context.Product.OrderByDescending(p => p.Id).ToList();
            

            ViewBag.Products = products;

            return View();
        }
    }
}
