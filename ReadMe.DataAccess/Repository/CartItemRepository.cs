using ReadMe.DataAccess.Data;
using ReadMe.DataAccess.Repository.IRepository;
using ReadMe.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReadMe.DataAccess.Repository
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private readonly ApplicationDbContext _db;
        public CartItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CartItem cartItem)
        {
           _db.CartItems.Update(cartItem);
        }
    }
}
