using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Data.DBInitializer
{
    public interface IDBInitializer
    {
        public Task Initialize();
    }
}
