using BakeryStoreMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BakeryStoreMVC.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {


        }

        public DbSet<Product>Product {  get; set; }

    }
}
