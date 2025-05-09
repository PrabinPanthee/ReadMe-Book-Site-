using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMe.DataAccess.Repository.IRepository
{
    public interface IUnitOfWorkcs
    {
        ICategoryRepository categoryRepository { get; }
        IProductRepository productRepository { get; }
        ICartRepository cartRepository { get; }
        ICartItemRepository cartItemRepository { get; }

        void Save();
    
    }
}
