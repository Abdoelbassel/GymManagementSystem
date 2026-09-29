using GymManagment.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Configurtions
{
    public class SessionConfiguring : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(x =>
            {
                x.HasCheckConstraint(
                    "Session_Check",
                    "StartDate < EndDate");

                x.HasCheckConstraint(
                    "Session_Check_Capacity",
                    "Capacity BETWEEN 1 AND 25");
            });
        }
    }
}
