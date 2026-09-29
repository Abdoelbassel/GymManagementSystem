using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagmentSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatedCategorisName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "Session_Check",
                table: "Sessions");

            migrationBuilder.DropCheckConstraint(
                name: "Session_Check_Capacity",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "CategoryName");

            migrationBuilder.AlterColumn<int>(
                name: "Capacity",
                table: "Sessions",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddCheckConstraint(
                name: "Session_Check",
                table: "Sessions",
                sql: "StartDate < EndDate");

            migrationBuilder.AddCheckConstraint(
                name: "Session_Check_Capacity",
                table: "Sessions",
                sql: "Capacity BETWEEN 1 AND 25");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "Session_Check",
                table: "Sessions");

            migrationBuilder.DropCheckConstraint(
                name: "Session_Check_Capacity",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Categories",
                newName: "Name");

            migrationBuilder.AlterColumn<decimal>(
                name: "Capacity",
                table: "Sessions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddCheckConstraint(
                name: "Session_Check",
                table: "Sessions",
                sql: "startDate < endDate");

            migrationBuilder.AddCheckConstraint(
                name: "Session_Check_Capacity",
                table: "Sessions",
                sql: "capacity between 1 and 25");
        }
    }
}
