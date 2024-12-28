using System.Diagnostics;
using BakeryStoreMVC.Models;
using BakeryStoreMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BakeryStoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var products = context.Product.OrderByDescending(p => p.Id).Take(4).ToList();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
