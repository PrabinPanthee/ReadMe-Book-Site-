using Microsoft.AspNetCore.Mvc;
using ReadMe.DataAccess.Repository.IRepository;
using ReadMe.Models;
using ReadMe.Models.Models;
using System.Diagnostics;

namespace ReadMe.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWorkcs _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWorkcs unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products =_unitOfWork.productRepository.GetAll(includeProperties: "Category").ToList();

            return View(products);
        }
        public IActionResult Details(int id)
        {
            Product product = _unitOfWork.productRepository.Get(u => u.ProductId == id, includeProperties:"Category");
            return View(product);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
