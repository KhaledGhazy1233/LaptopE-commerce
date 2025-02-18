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
namespace Tech.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    { 
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) :base(db)
        {
        _db = db;
        }   
        

        public void Update(Product obj)
        {
           _db.Update(obj);
        }
    }
}
