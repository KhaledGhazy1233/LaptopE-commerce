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
    public class WishListItemRepository: Repository<WishListItem>, IWishListItemRepository
    {
        private ApplicationDbContext _db;
        public WishListItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
       
    }
}
