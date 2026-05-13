using CoffeeModel;
using Microsoft.EntityFrameworkCore;

namespace CoffeeData
{
    public class CoffeeDb : DbContext
    {
        public CoffeeDb(DbContextOptions<CoffeeDb> options)
        : base(options) { }

        public DbSet<Coffee> Coffees { get; set; }
        
    }
}