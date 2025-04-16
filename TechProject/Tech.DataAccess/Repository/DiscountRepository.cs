using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tech.DataAccess.Repository.IRepository;
using Tech.Models.Models;
using TechProject.DataAccess.Data;
using TechProject.Models;

namespace Tech.DataAccess.Repository
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        private readonly ApplicationDbContext _db;
        public DiscountRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        


        public void Update(Discount obj)
        {
            _db.Update(obj);
        }
    }
}
