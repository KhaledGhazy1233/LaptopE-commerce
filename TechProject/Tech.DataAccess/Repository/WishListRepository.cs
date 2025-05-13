using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.DataAccess.Repository.IRepository;
using Tech.Models.Models;
using TechProject.DataAccess.Data;

namespace Tech.DataAccess.Repository
{
    public class WishListRepository:Repository<WishList>,IWishListRepository
    {
        private ApplicationDbContext _db;
        public WishListRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
     
    }
   
}
