using Microsoft.EntityFrameworkCore;
using ReadMe.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMe.DataAccess.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options ): base(options)
        {
        }

       public DbSet<Category> categories {  get; set; }
       public DbSet<Product> products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Title = "Action", Description="Hello",ISBN="56464"
                ,Author="Prabin",ListPrice=20,Price=17.5,Price50=16.5,Price100=16}
                
                );
        }

    }
}
