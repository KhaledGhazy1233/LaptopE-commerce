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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    { 
        private ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) :base(db)
        {
        _db = db;
        }   
        

        public void Update(OrderHeader obj)
        {
           _db.Update(obj);
        }
    }
}
