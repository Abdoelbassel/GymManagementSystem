using GymManagment.DAL.Data.Context;
using GymManagment.DAL.DataSeed;
using GymManagmentSystem.DAL.DataSeed;
using GymManagmentSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.Extentions
{
    public static class ProgramExtenions
    {
        public static async Task MigrateAndSeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<GymAppContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var pendingMigration = await dbcontext.Database.GetPendingMigrationsAsync();
            if (pendingMigration.Any())
            {
                dbcontext.Database.Migrate();
            }

            var folderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await DataBaseSeeder.SeedAsync(dbcontext, folderPath, logger);
            await IdentityDataSeed.SeedIdentityDataAsync(roleManager, userManager, logger);
        }
    }
}
