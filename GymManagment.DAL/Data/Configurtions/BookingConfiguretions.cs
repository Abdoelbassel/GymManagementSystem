using GymManagment.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.DAL.Data.Configurtions
{
    internal class BookingConfiguretions : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(b => b.Id);

            builder.Property(c => c.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(b => b.Member)
                .WithMany(b => b.Booking)
                .HasForeignKey(b => b.MemberId);

            builder.HasOne(b => b.Session)
                .WithMany(b => b.Bookings)
                .HasForeignKey(b => b.SessionId);

            builder.HasKey(b => new { b.MemberId, b.SessionId });
        }
    }
}
