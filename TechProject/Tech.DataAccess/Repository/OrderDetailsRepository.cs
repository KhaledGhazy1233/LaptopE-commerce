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
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    { 
        private ApplicationDbContext _db;
        public OrderDetailsRepository(ApplicationDbContext db) :base(db)
        {
        _db = db;
        }   
        

        public void Update(OrderDetails obj)
        {
           _db.Update(obj);
        }
    }
}
