using System.ComponentModel.DataAnnotations;

namespace TechProject.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required , MaxLength(20)]
        public string ? Name { get; set; }
        [Range(1,100,ErrorMessage="Must be between 1-to100")]
        public int DisplayOrder { get; set; }
    }
}
