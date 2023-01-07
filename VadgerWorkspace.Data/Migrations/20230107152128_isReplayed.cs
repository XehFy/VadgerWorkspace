using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VadgerWorkspace.Data.Migrations
{
    /// <inheritdoc />
    public partial class isReplayed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReplayed",
                table: "Clients",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReplayed",
                table: "Clients");
        }
    }
}
