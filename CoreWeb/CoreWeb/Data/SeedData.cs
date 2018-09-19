using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb.Data
{
    public class SeedData
    {
        private static readonly string[] roles = new[] {
            "Administrator",
            "Manager",
            "User"
        };

        public async Task SeedDataAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string email = "admin@test.com";
            string roleName = "Administrator";
            string password = "Pa$$word1"; //THAT's really secure!!!

            //Check that there is an Administrator role and create if not
            foreach (var role in roles)
            {

                if (!await roleManager.RoleExistsAsync(role))
                {
                    var create = await roleManager.CreateAsync(new IdentityRole(role));

                    if (!create.Succeeded)
                    {

                        throw new Exception("Failed to create role");

                    }
                }

            }

            //Check if the admin user exists and create it if not
            //Add to the Administrator role
            IdentityUser testUser = await userManager.FindByEmailAsync(email);


            if (testUser == null)
            {
                IdentityUser administrator = new IdentityUser();
                administrator.Email = email;
                administrator.UserName = email;

                IdentityResult newUser = await userManager.CreateAsync(administrator, password);

                if (newUser.Succeeded)
                {
                    IdentityResult newUserRole = await userManager.AddToRoleAsync(administrator, roleName);
                }
            }

        }
    }
}
