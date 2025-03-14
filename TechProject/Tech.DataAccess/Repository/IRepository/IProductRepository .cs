using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechProject.Models;
using TechProject.DataAccess.Data;
using Tech.Models.Models;
namespace Tech.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product> 
    {
        void Update(Product obj);
        void AddProductImages(int productId, List<ProductImage> images);
        bool DeleteProduct(int productId);
        bool DeleteImages(List<ProductImage> images);
    }
}
