using Domin.Models.identitys;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DataSeeding
{
    public static class SeedingRole
    {
        public async static void SeedingUser(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContexts>();
            var manager = services.GetRequiredService<UserManager<AppUser>>();

            //Get All Roles
            var RoleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            string[] Roles = { "Customer", "Admin" };
            //Check if Roles Exist and Add it 
            foreach (var Role in Roles)
            {
                if (!await RoleManager.RoleExistsAsync(Role))
                {
                    await RoleManager.CreateAsync(new IdentityRole(Role));
                }
            }
            //Adding Admin 

            if (!await context.UserRoles.AnyAsync())
            {
                // in this Case Create Two Users

                var SuperAdmin = new AppUser()
                {

                    UserName = "Omar_Ahmed",
                    Email = "SuperAdmin@gmail.com",
                    PhoneNumber = "01004550656"
                };

                var Admin = new AppUser()
                {
                    UserName = "Mohamed_Hesham",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "01005557656"
                };

                await manager.CreateAsync(SuperAdmin, "SuperAdmin@123");
                await manager.CreateAsync(Admin, "Admin@123");

                //Adding Roles to Users
                await manager.AddToRoleAsync(SuperAdmin, "Admin");
                await manager.AddToRoleAsync(Admin, "Admin");
            }
        }
    }
}
