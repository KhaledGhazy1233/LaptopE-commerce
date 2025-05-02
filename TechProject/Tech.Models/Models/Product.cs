using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechProject.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Tech.Models.Models;

namespace TechProject.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

        [StringLength(255)]
        public string? Brand { get; set; }

        [StringLength(255)]
        public string? Processor { get; set; }

        [Range(1, 256)]
        public int RAM { get; set; }

        [StringLength(255)]
        public string? Storage { get; set; }

        [StringLength(255)]
        public string? GPU { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal ScreenSize { get; set; }

        [StringLength(50)]
        public string? Resolution { get; set; }

        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public double? PriceAfterDiscount { get; set; }
        public double? Presentage { get; set; }
        public  List<ProductImage?> ProductImages { get; set; } = new List<ProductImage>();
        public string? headerImageUrl { get; set; }


    }
}
