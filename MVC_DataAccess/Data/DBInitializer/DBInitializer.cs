using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC.DataAccess.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MVC.DataAccess.Data.DBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DatabaseContext _dbcontext;

        public DBInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            DatabaseContext dbcontext)
        {
            _roleManager = roleManager;
            _userManager = userManager;            
            _dbcontext = dbcontext;

        }
        async Task IDBInitializer.Initialize()
        {
            try
            {
                if(_dbcontext.Database.GetPendingMigrations().Count() > 0)
                {
                    _dbcontext.Database.Migrate();
                }
                

                
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error while applying migrations {e.Message} ");
                //throw;
            }

            if (!_roleManager.RoleExistsAsync(Roles.User).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                await _roleManager.CreateAsync(new IdentityRole(Roles.User));

                _userManager.CreateAsync(new IdentityUser
                {
                    UserName = "admin@gmail.com"
                }, "Admin123!").GetAwaiter().GetResult();

                IdentityUser Admin = _dbcontext.Users.FirstOrDefault(x => x.UserName == "admin@gmail.com");
                _userManager.AddToRoleAsync(Admin, Roles.Admin).GetAwaiter().GetResult();
            }
            return;

            


        }
    }
}
