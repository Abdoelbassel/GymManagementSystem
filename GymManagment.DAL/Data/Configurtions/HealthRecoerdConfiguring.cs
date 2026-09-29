using GymManagment.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Configurtions
{
    public class HealthRecoerdConfiguring : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.Property(hr => hr.Weight)
                   .IsRequired()
                   .HasColumnType("decimal(5, 2)");

            builder.Property(hr => hr.Height)
                   .IsRequired()
                   .HasColumnType("decimal(5, 2)");

            builder.Property(hr => hr.Note)
                   .HasMaxLength(500);

            builder.Property(hr => hr.BloodType)
                   .IsRequired()
                   .HasMaxLength(5);
        }
    }
}
