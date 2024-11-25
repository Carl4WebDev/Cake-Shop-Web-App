using BakeryStoreMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BakeryStoreMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;

        public ProductsController(ApplicationDbContext context)
        {
            this.context = context;
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
    }
}
