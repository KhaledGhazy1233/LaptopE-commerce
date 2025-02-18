using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechProject.Models;
using TechProject.DataAccess.Data;
namespace Tech.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product> 
    {
        void Update(Product obj);
    }
}
