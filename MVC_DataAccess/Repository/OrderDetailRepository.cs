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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
	{
        private readonly DatabaseContext _db;

        public OrderDetailRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }        

        void IOrderDetailRepository.Update(OrderDetail orderDetail)
        {
            _db.orderDetail.Update(orderDetail);
        }
    }
}
