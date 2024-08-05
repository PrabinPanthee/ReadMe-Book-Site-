using Microsoft.AspNetCore.Mvc;
using ReadMe.DataAccess.Repository.IRepository;

namespace ReadMe.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWorkcs _unit;
        public ProductController(IUnitOfWorkcs unit)
        {
            _unit = unit;
        }
        public IActionResult Index()
        {
           var products = _unit.productRepository.GetAll().ToList();
            return View(products);
        }
    }
}
