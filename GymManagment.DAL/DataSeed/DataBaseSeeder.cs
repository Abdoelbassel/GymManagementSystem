using GymManagment.DAL.Data.Context;
using GymManagment.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
namespace GymManagment.DAL.DataSeed
{
    public static class DataBaseSeeder
    {

        public static async Task SeedAsync(GymAppContext context , string SeedFolderPath , ILogger logger , CancellationToken ct = default)
        {
            try
            {
                if(! await context.Plans.AnyAsync())
                {
                    var plans = LoadFromDataJsonFile<PLan>(SeedFolderPath, "plans.json");
                    if (plans.Any())
                        context.Plans.AddRange(plans);

                    if (context.ChangeTracker.HasChanges())
                        await context.SaveChangesAsync();

                    else
                        logger.LogInformation("Plan Already Seeded");
                }
            }
            catch(Exception ex)
            {
                logger.LogInformation($"Failed to load {SeedFolderPath}");
                throw;
            }
        }

        private static List<T> LoadFromDataJsonFile<T> (string FolderPath , string fileName)
        {
            var filePath = Path.Combine(FolderPath , fileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File Path Not Found {filePath}");

            var data = File.ReadAllText(filePath);
            var option = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            return JsonSerializer.Deserialize<List<T>>(data , option) ?? [];
            
        }
    }
}