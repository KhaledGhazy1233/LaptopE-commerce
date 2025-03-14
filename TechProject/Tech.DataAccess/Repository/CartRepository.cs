using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.DataAccess.Repository;
using Tech.DataAccess.Repository.IRepository;
using Tech.Models.Models;
using TechProject.DataAccess.Data;
using TechProject.Models;

namespace Tech.DataAccess.Repository
{


    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _db;

        public CartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

       
        public void Update(Cart cart)
        {
            _db.Update(cart);
        }
    }
}