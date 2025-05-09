using ReadMe.DataAccess.Repository.IRepository;
using ReadMe.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMe.DataAccess.Repository
{
    public interface ICartRepository:IRepository<Cart>
    {
        void Update(Cart cart);
    }
}
