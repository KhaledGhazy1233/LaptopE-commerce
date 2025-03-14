using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Models.Models;
using TechProject.Models;

namespace Tech.Models.ViewModels
{
    public class ProductVM
    {
        public Product ?product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ?CategoryList { get; set; }
        public IEnumerable<ProductImage> ? ProductImages { get; set; } = new List<ProductImage>();
        public List<IFormFile> ? Files { get; set; }=new List<IFormFile?>();
    }
}
