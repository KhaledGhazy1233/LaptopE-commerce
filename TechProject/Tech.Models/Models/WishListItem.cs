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
    public class WishListItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int WishlistId { get; set; }

        [ForeignKey("WishlistId")]
        public WishList Wishlist { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
