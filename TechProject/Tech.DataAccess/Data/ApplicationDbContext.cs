using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tech.Models.Models;
using TechProject.Models;
namespace TechProject.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductImage>()
           .HasOne(p => p.Product)
           .WithMany(p => p.ProductImages)
           .HasForeignKey(p => p.ProductId);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Laptops", Description = "Various types of laptops" },
                new Category { Id = 2, Name = "Gaming Laptops", Description = "High-performance gaming laptops" },
                new Category { Id = 3, Name = "Accessories", Description = "Laptop accessories like bags, coolers, and docks" },
                new Category { Id = 4, Name = "Ultrabooks", Description = "Lightweight, portable laptops" },
                new Category { Id = 5, Name = "Workstations", Description = "High-end computers for professional use" }
           );
            modelBuilder.Entity<Product>().HasData(
      new Product
      {
          Id = 1,
          Name = "Dell Inspiron 15",
          CategoryId = 1, // Laptops
          Brand = "Dell",
          Processor = "Intel i7",
          RAM = 16,
          Storage = "512GB SSD",
          GPU = "NVIDIA GTX 1650",
          ScreenSize = 15.6m,
          Resolution = "1920x1080",
          Price = 799.99m,
          Description = "A reliable laptop for everyday use."
          
      },
      new Product
      {
          Id = 2,
          Name = "HP Pavilion Gaming",
          CategoryId = 2, // Gaming Laptops
          Brand = "HP",
          Processor = "Intel i5",
          RAM = 8,
          Storage = "1TB HDD",
          GPU = "NVIDIA GTX 1650",
          ScreenSize = 15.6m,
          Resolution = "1920x1080",
          Price = 999.99m,
          Description = "Gaming laptop with great performance."
         
      },
      new Product
      {
          Id = 3,
          Name = "MacBook Air 13",
          CategoryId = 1, // Laptops
          Brand = "Apple",
          Processor = "M1 Chip",
          RAM = 8,
          Storage = "256GB SSD",
          GPU = "Integrated",
          ScreenSize = 13.3m,
          Resolution = "2560x1600",
          Price = 999.99m,
          Description = "Thin, light, and powerful MacBook."
         
      },
      new Product
      {
          Id = 4,
          Name = "Lenovo Yoga 730",
          CategoryId = 1, // Laptops
          Brand = "Lenovo",
          Processor = "Intel i7",
          RAM = 16,
          Storage = "512GB SSD",
          GPU = "Intel UHD Graphics 620",
          ScreenSize = 13.3m,
          Resolution = "1920x1080",
          Price = 1099.99m,
          Description = "2-in-1 laptop with a sleek design."
         
      },
      new Product
      {
          Id = 5,
          Name = "Corsair Gaming Mouse",
          CategoryId = 3, // Accessories
          Brand = "Corsair",
          Processor = null,
          RAM = 0,
          Storage = null,
          GPU = null,
          ScreenSize = 0,
          Resolution = null,
          Price = 49.99m,
          Description = "High-precision gaming mouse."
          
      }
         );
        }
   


        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<WishList> Wishlists { get; set; }
        public DbSet<WishListItem> WishlistItems { get; set; }
    }
}
