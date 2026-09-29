using GymManagment.DAL.Data.Context;
using GymManagment.DAL.DataSeed;
using GymManagmentSystem.BLL;
using GymManagmentSystem.BLL.Extentions;
using GymManagmentSystem.BLL.Services.Attachment;
using GymManagmentSystem.BLL.Services.Classes;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.DAL.Models;
using GymManagmentSystem.DAL.Repositorities.Classes;
using GymManagmentSystem.DAL.Repositorities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagmentSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped(typeof(IGenaricRepositorities<>), typeof(GenaricRepositorities<>));

            builder.Services.AddScoped<IMemberServices, MemberServices>();

            builder.Services.AddScoped<IPlanServices, PlanServices>();
            builder.Services.AddScoped<ITrainerServices, TrainerServices>();
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddAutoMapper(a => a.AddProfile(new MappingMapper()));
            builder.Services.AddScoped<ISessionRepo , SessionRepo>();
            builder.Services.AddScoped<ISessionServices , SessionServices>();
            builder.Services.AddScoped<IMembershipRepo, MembershipRepo>();
            builder.Services.AddScoped(typeof(IMembershipServices), typeof(MembershipServices));
            builder.Services.AddScoped<IBookingRepo, BookingRepo>();
            builder.Services.AddScoped<IBookingSevices, BookingServices>();
            builder.Services.AddScoped<IAnalyticsServices, AnalyticsServices>();
            builder.Services.AddScoped<IAttachmentServices, AttachmentServices>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                config.Lockout.MaxFailedAccessAttempts = 5;
            }).AddEntityFrameworkStores<GymAppContext>();
            builder.Services.AddDbContext<GymAppContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();

            await app.MigrateAndSeedDataAsync();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            //using (var scope = app.Services.CreateScope())
            //{
            //    var context = scope.ServiceProvider.GetRequiredService<GymAppContext>();

            //    await DataBaseSeeder.SeedAllAsync(context);
            //}
            app.Run();
        }
    }
}
