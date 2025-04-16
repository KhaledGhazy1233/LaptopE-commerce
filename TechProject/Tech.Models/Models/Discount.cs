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
    public class Discount
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double Presentage { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsCompitaple { get; set; } = false;
        [Required]
        [ForeignKey(nameof(Product))]
         public int? ProductId { get; set; }
        public Product? Product { get; set; }



    }
}
