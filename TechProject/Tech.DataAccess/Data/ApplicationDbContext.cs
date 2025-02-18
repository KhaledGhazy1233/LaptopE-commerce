using Microsoft.EntityFrameworkCore;
using TechProject.Models;
namespace TechProject.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
             .HasData(
                 new Category {CategoryId =1 ,Name= "Action", DisplayOrder =1},
                 new Category {CategoryId =2 ,Name= "Scifi", DisplayOrder =2},
                 new Category {CategoryId =3 ,Name= "History", DisplayOrder =3}
                
                );                
        }

      

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> MyProperty { get; set; }
    }
}
