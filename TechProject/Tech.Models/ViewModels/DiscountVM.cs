using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tech.Models.Models;

namespace Tech.Models.ViewModels
{
    public class DiscountVM
    {

        public Discount discount { get; set; }
        [ValidateNever]
        public List<SelectListItem> products { get; set; }
    }
}
