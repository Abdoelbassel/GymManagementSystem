using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagmentSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatedCreateing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Gander",
                table: "Trainers",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "Gander",
                table: "Sessions",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "Gander",
                table: "Members",
                newName: "Gender");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Trainers",
                newName: "Gander");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Sessions",
                newName: "Gander");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Members",
                newName: "Gander");
        }
    }
}
