﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Models.Models;
using TechProject.Models;

namespace Tech.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart>? ShoppingCartList { get; set; }
        public OrderHeader? OrderHeader { get; set; }

    }
}
