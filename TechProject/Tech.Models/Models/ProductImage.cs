using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechProject.Models;

namespace Tech.Models.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        public string ImageUrl { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
       
        public  Product Product { get; set; }
    }
}
