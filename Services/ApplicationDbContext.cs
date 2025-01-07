using BakeryStoreMVC.Models;
using BestStoreMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BakeryStoreMVC.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {


        }

        public DbSet<Product>Product {  get; set; }
        public DbSet<Order> Orders { get; set; }
        //public object Products { get; internal set; }
    }
}
