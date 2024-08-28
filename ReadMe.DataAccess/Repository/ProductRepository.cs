using ReadMe.DataAccess.Data;
using ReadMe.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReadMe.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
            
        }

        public void Update(Product product)
        {
            var objFromDb = _db.products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if ((objFromDb != null))
            {
                objFromDb.Title = product.Title;
                objFromDb.Description = product.Description;
                objFromDb.Author = product.Author;
                objFromDb.ISBN = product.ISBN;
                objFromDb.CategoryId = product.CategoryId;
                objFromDb.Discount = product.Discount;
                objFromDb.ListPrice = product.ListPrice;
                
                if (product.ImageUrl != null) 
                {
                    objFromDb.ImageUrl = product.ImageUrl;                
                }
                
                
            }
        }
    }
}
