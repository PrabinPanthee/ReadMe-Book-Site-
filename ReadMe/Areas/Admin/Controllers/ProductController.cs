using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReadMe.DataAccess.Repository.IRepository;
using ReadMe.Models.Models;
using ReadMe.Models.ViewModels;

namespace ReadMe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWorkcs _unit;
        public ProductController(IUnitOfWorkcs unit, IWebHostEnvironment webHostEnvironment)
        {
            _unit = unit;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
           var products = _unit.productRepository.GetAll("Category").ToList();
         
            return View(products);
        }


        //Basically we can just make a same controller and view for create and edit 
        //update and insert == upsert
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ProductVm productVm = new()
            {
               
                CategoryList = _unit.categoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };

            if (id==null || id == 0)
            {
                return View(productVm);
            }
            else
            {
                productVm.Product =_unit.productRepository.Get(u=>u.ProductId == id);
                return View(productVm);
            }
            

        }




        //Normal way
        //public IActionResult Create()
        //{
        //    //IEnumerable<SelectListItem> CategoryList = _unit.categoryRepository.GetAll().Select(u => new SelectListItem
        //    //{
        //    //    Text = u.Name,
        //    //    Value = u.Id.ToString()
        //    //});
        //    //ViewBag.CategoryList = CategoryList;
        //    //ViewData["CategoryList"] = CategoryList;
        //    ProductVm product = new()
        //    {
        //        //CategoryList = CategoryList,
        //        CategoryList = _unit.categoryRepository.GetAll().Select(u => new SelectListItem
        //        {
        //            Text = u.Name,
        //            Value = u.Id.ToString()
        //        }),
        //        Product = new Product()
        //    };
        //    return View(product);

        //}
        [HttpPost]
        public IActionResult Upsert(ProductVm obj,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images/product");
                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) 
                        {
                         System.IO.File.Delete(oldImagePath);
                        }

                    }
                  

                    using(var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create)) 
                    {
                      file.CopyTo(fileStream);

                    }

                    obj.Product.ImageUrl = @"\Images\Product\" + filename;



                }

                if (obj.Product.ProductId == 0)
                { 
                    _unit.productRepository.Add(obj.Product);
                }
                else
                { 
                    _unit.productRepository.Update(obj.Product);
                }

                _unit.Save();
                if (obj.Product.ProductId == 0)
                {
                    TempData["Success"] = "Product created successfully";
                }
                else 
                {
                    TempData["Success"] = "Product Edited successfully";
                }
                
                return RedirectToAction("Index");


            }
            else

            {   //to ignore exception
                obj.CategoryList = _unit.categoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(obj);

            }


        }


        //normal Create
        //public IActionResult Create(ProductVm obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unit.productRepository.Add(obj.Product);
        //        _unit.Save();
        //        TempData["Success"] = "Product created successfully";
        //        return RedirectToAction("Index");


        //    }
        //    else
        //    {   //to ignore exception
        //        obj.CategoryList = _unit.categoryRepository.GetAll().Select(u => new SelectListItem
        //        {
        //            Text = u.Name,
        //            Value = u.Id.ToString()
        //        });
        //        return View(obj);

        //    }


        //}
        //[HttpGet]
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    var product = _unit.productRepository.Get(p => p.ProductId == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}
        //[HttpPost]
        //public IActionResult Edit(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
                

        //        _unit.productRepository.Update(product);
        //        _unit.Save();
        //        TempData["success"] = "Product Updated Successfully";
        //        return RedirectToAction("Index");

        //    }
        //    return View();

        //}

       
      
       //web api endpoints

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unit.productRepository.GetAll(includeProperties:"Category").ToList();

            return Json(new {data = products});
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
           var productToBeDeleted = _unit.productRepository.Get(p => p.ProductId == id);
            if (productToBeDeleted is null) 
            { 
                return Json(new {res = false, message ="Product Not Found"}); 
            }
            string wwwRootPath = _webHostEnvironment.WebRootPath;
           
            var oldImagePath = Path.Combine(wwwRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
           
            _unit.productRepository.Remove(productToBeDeleted);
            _unit.Save();
            return Json(new {res=true,message="successfully deleted"});

        }
        #endregion



    }
}
