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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly DatabaseContext _db;

        public CategoryRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }        

        void ICategoryRepository.Update(Category category)
        {
            _db.categories.Update(category);
        }
    }
}
