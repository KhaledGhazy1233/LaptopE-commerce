using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechProject.Models;

namespace TechProject.Models
{
    public class Product
    {
        [Key]  // This is for Primary Key
        public int Id { get; set; }

        [Required, MaxLength(255)]  // Marks this field as mandatory
        [StringLength(255)]  // Limits the length of the string
        public string? Name { get; set; }

        [ForeignKey("Category")]  // Foreign Key to the Category table
        public int CategoryId { get; set; }

        public Category? Category { get; set; }  // Navigation property

        [MaxLength(255)]  // Limits the length of the string
        public string? Brand { get; set; }

        [MaxLength(255)]  // Limits the length of the string
        public string? Processor { get; set; }

        public int RAM { get; set; }  // Size of RAM in GB

        [MaxLength(255)]  // Limits the length of the string
        public string? Storage { get; set; }  // Storage type and capacity

        [StringLength(255)]  // Limits the length of the string
        public string? GPU { get; set; }

        public decimal ScreenSize { get; set; }  // Size of the screen in inches

        [MaxLength(50)]  // Limits the length of the string
        public string? Resolution { get; set; }  // Resolution of the screen

        [Range(0, double.MaxValue)]  // Ensures the price is positive
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }  // Quantity of the product in stock

        public string? Description { get; set; }  // Product description

        [StringLength(500)]  // Limits the length of the URL string
        public string? ImageUrl { get; set; }

    }
}
