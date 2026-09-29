using GymManagment.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Configurtions
{
    public abstract class GymUserConfiguring<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .IsRequired(false);

            builder.Property(x => x.Phone)
                .HasColumnType("char")
                .HasMaxLength(11);

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.City)
                    .HasColumnType("varchar")
                    .HasMaxLength(50);


                address.Property(a => a.Street)
                    .HasColumnType("varchar")
                    .HasMaxLength(100);
            });

            builder.ToTable(x =>
            {
                x.HasCheckConstraint("Email_Check", "[Email] LIKE '%@%.%'");
                x.HasCheckConstraint("Phone_Check", "[Phone] LIKE [0-9] AND LEN([Phone]) = 11");
            });
        }
    }
}