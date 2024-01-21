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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly DatabaseContext _db;

        public ShoppingCartRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }        

        void IShoppingCartRepository.Update(ShoppingCart cart)
        {
            _db.shoppingCarts.Update(cart);
        }
    }
}
