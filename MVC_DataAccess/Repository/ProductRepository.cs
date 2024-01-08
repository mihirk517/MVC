using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository
{
    public class ProductRepository :Repository<Product>,IProductRepository
    {
        private readonly DatabaseContext _db;

        public ProductRepository(DatabaseContext db) :base(db)
        {
            _db = db;
        }

        void IProductRepository.Update(Product product)
        {
            _db.products.Update(product);
        }

    }
}
