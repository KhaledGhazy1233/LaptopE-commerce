using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.DataAccess.Repository.IRepository;
using TechProject.DataAccess.Data;
using TechProject.Models;

namespace Tech.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext ? _db;
        public ICategoryRepository? Category { get; private set; }
        public IProductRepository ? Product { get; set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category= new CategoryRepository(_db);
            Product= new ProductRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
