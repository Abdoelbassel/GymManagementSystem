using GymManagment.DAL.Data.Configurtions;
using GymManagment.DAL.Models;
using GymManagmentSystem.DAL.Data.Configurtions;
using GymManagmentSystem.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GymManagment.DAL.Data.Context
{
    public class GymAppContext : IdentityDbContext<ApplicationUser>
    {
        public GymAppContext(DbContextOptions<GymAppContext> options)
        : base(options)
        {
        }
        public DbSet<PLan> Plans { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Membership> Memberships { get; set; }


        //override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer("Server=.;Database=GymManagmentApp;Trusted_Connection=true;TrustServerCertificate=true;");
        //}
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<ApplicationUser>(a =>
            {
                a.Property(f => f.FirstName)
                .HasColumnType("varchar")
                .HasMaxLength(50);

                a.Property(l => l.LastName)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            });
                
            

        }
    }
}
