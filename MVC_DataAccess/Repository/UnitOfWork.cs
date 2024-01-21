using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _dbContext;
        public ICategoryRepository CategoryRepository { get; private set; }
        public IShoppingCartRepository ShoppingCartRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }

		public IOrderHeaderRepository OrderHeaderRepository { get; private set; }

		public IOrderDetailRepository OrderDetailRepository { get; private set; }

		public UnitOfWork(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            CategoryRepository = new CategoryRepository(_dbContext);
            ProductRepository = new ProductRepository(_dbContext);
            ShoppingCartRepository = new ShoppingCartRepository(_dbContext);
            OrderHeaderRepository = new OrderHeaderRepository(_dbContext);
            OrderDetailRepository = new OrderDetailRepository(_dbContext);
        }
   

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
