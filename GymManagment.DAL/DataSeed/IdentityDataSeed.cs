using GymManagmentSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace GymManagmentSystem.DAL.DataSeed
{
    public static  class IdentityDataSeed
    {
        public static async Task SeedIdentityDataAsync (RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager , ILogger logger , CancellationToken ct = default)
        {
            try
            {
                bool hasUsers = await userManager.Users.AnyAsync(ct);
                bool hasRoles = await roleManager.Roles.AnyAsync(ct);

                if (hasRoles && hasUsers) return;

                var roles = new List<IdentityRole>()
            {
                new IdentityRole("SuperAdmin"),
                new IdentityRole("Amdin")
            };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name))
                    {
                        var roleResult = await roleManager.CreateAsync(role);

                        if (!roleResult.Succeeded)
                        {
                            logger.LogError($"Faild To Create Role {role.Name} : {string.Join(" , ", roleResult.Errors.Select(e => e.Description))}");
                        }
                    }
                }

                if (!hasUsers)
                {
                    var mainAdmin = new ApplicationUser()
                    {
                        FirstName = "Abdullrahman",
                        LastName = "Elbassel",
                        Email = "abdoelbassel@gmail.com",
                        UserName = "AbdoElbassel",
                        PhoneNumber = "01025578701",

                    };

                    await userManager.CreateAsync(mainAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(mainAdmin, "SuperAdmin");

                    var admin = new ApplicationUser()
                    {
                        FirstName = "Ahmed",
                        LastName = "Elbassel",
                        Email = "ahmedelbassel@gmail.com",
                        UserName = "abdoELassel",
                        PhoneNumber = "01002999564"
                    };

                    await userManager.CreateAsync(admin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(admin, "Admin");

                    logger.LogInformation("Identity Data Seeded");

                }

                return;
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Identit Seeding Faild");
                return;
            }
        }
    }
}


