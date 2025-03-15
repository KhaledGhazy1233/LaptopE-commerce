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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    { 
        private ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) :base(db)
        {
        _db = db;
        }   
        

        public void Update(ShoppingCart obj)
        {
           _db.Update(obj);
        }
    }
}
