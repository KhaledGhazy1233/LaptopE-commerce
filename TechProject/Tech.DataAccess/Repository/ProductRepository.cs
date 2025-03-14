using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tech.DataAccess.Repository.IRepository;
using TechProject.DataAccess.Data;
using TechProject.Models;
using Microsoft.EntityFrameworkCore;
using Tech.Models.Models;
namespace Tech.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    { 
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) :base(db)
        {
        _db = db;
        }

        public void  AddProductImages(int productId, List<ProductImage> images)
        {
            var product = _db.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == productId);
            if (product !=null) 
            {
                _db.ProductImages.AddRange(images);
                _db.SaveChanges();
            }
        }

        public bool DeleteImages(List<ProductImage> images)
        {
            foreach (var image in images)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImageUrl.TrimStart('\\', '/'));
                filePath = Path.GetFullPath(filePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }


            _db.ProductImages.RemoveRange(images);
            _db.SaveChanges();
            return true;
        }

        public bool DeleteProduct(int productId)
        {
            var product = _db.Products.Include(p => p.ProductImages)
                                          .FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {


                if (DeleteImages(product.ProductImages))
                {
                    _db.Products.Remove(product);

                    _db.SaveChanges();
                    return true;
                }
                else return false;


            }
            else
            {
                return false;
            }
        }


        public void Update(Product obj)
        {
           _db.Update(obj);
        }

       
    }
}
