using BDS_ML.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Data
{
    public class DummyData
    {
        public static async Task Initialize(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            string adminId1 = "";
            string adminId2 = "";

            string role1 = "Admin";
            string desc1 = "This is the Administration role";
            string role2 = "User";
            string desc2 = "This is the User role";

            string password = "123";

            if (await roleManager.FindByNameAsync(role1) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role1, desc1, DateTime.Now));
            }
            if (await roleManager.FindByNameAsync(role2) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role2, desc2, DateTime.Now));
            }


            if (await userManager.FindByNameAsync("aa@aa.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "aa@aa.com",
                    Email = "aa@aa.com",
                    FirstName = "AA",
                    LastName = "aa",
                    City = "Tp.HCM",
                    PhoneNumber = "012232321",
                    Address = "Ho Ba Phan, Q.9",
                    IsAdmin = 1,
                    IsBlock = 0,
                   

                };
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role1);

                }
                adminId1 = user.Id;
            }
            if (await userManager.FindByNameAsync("bb@bb.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "bb@bb.com",
                    Email = "bb@bb.com",
                    FirstName = "BB",
                    LastName = "bb",
                    City = "Tp.HCM",
                    PhoneNumber = "01gjgj32321",
                    Address = "Nguyen Thi Minh Khai, Q.1",
                    IsAdmin = 0,
                    IsBlock = 0

                };
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);

                }
                adminId2 = user.Id;
            }



        }
    }
}
