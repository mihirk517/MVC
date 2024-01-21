using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly DatabaseContext _db;

        public OrderHeaderRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }        

        void IOrderHeaderRepository.Update(OrderHeader orderHeader)
        {
            _db.orderHeaders.Update(orderHeader);
        }
    }
}
