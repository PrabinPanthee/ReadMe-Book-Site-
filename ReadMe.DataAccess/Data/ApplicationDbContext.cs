using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReadMe.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMe.DataAccess.Data
{
    public class ApplicationDbContext :IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options ): base(options)
        {
        }

       public DbSet<Category> categories {  get; set; }
       public DbSet<Product> products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //onModelCreating(modelBuilder);
        //    modelBuilder.Entity<Product>().HasData(
        //        new Product
        //        {
        //            ProductId = 1,
        //            Title = "",
        //            Description = "Hello",
        //            ISBN = "56464"
        //        ,
        //            Author = "Prabin",
        //            ListPrice = 20,
        //            Discount=20,
        //            CategoryId = 2,
        //            ImageUrl = ""
        //        },
        //         new Product
        //         {
        //             ProductId = 2,
        //             Title = "Action",
        //             Description = "Hi",
        //             ISBN = "54d4d",

        //             Author = "Vision",
        //             ListPrice = 20,
        //            Discount = 20,
        //             CategoryId = 3,
        //             ImageUrl = ""
        //         }

        //        );
        //}

    }
}
