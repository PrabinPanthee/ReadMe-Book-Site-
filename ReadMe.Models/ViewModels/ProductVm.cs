using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReadMe.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMe.Models.ViewModels
{
    public class ProductVm
    {
        public Product Product { get; set; }

        [ValidateNever] 
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
