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
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            categoryRepository = new CategoryRepository(db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
