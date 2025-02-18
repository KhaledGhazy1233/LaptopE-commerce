using System.ComponentModel.DataAnnotations;

namespace TechProject.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string ? Name { get; set; } // مثل Laptops, Accessories, Gaming

        public string? Description { get; set; }
        public List<Product> ? Products { get; set; }
    }
}
