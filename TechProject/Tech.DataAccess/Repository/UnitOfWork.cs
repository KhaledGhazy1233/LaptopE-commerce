﻿using System;
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
        public IShoppingCartRepository ShoppingCart { get; set; }

        public IApplicationUserRepository ApplicationUser { get; set; }
        public IOrderHeaderRepository OrderHeader { get; set; }
        public IOrderDetailsRepository OrderDetail { get; set; }
        public IDiscountRepository Discount { get; set; }

        public IWishListRepository WishList { get; set; }

        public IWishListItemRepository WishListItem { get; set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category= new CategoryRepository(_db);
            Product= new ProductRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailsRepository(_db);
            Discount = new DiscountRepository(_db);
            WishList = new WishListRepository(_db);
            WishListItem = new WishListItemRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
