using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VadgerWorkspace.Data.Migrations
{
    /// <inheritdoc />
    public partial class Stages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegistrationStage",
                table: "Clients",
                newName: "Stage");

            migrationBuilder.AddColumn<long>(
                name: "ClientId",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "Stage",
                table: "Clients",
                newName: "RegistrationStage");
        }
    }
}
