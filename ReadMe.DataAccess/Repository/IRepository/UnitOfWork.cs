using ReadMe.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMe.DataAccess.Repository.IRepository
{
    public class UnitOfWork : IUnitOfWorkcs
    {
        public ICategoryRepository categoryRepository {  get; private set; }

        public IProductRepository productRepository {  get; private set; }
        public ICartItemRepository cartItemRepository { get; private set; }

        public ICartRepository cartRepository { get; private set; }

        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            categoryRepository = new CategoryRepository(db);
            productRepository = new ProductRepository(db);
            cartItemRepository = new CartItemRepository(db);
            cartRepository = new CartRepository(db);

        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
