using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VadgerWorkspace.Data.Migrations
{
    /// <inheritdoc />
    public partial class clienttag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Clients",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Clients");
        }
    }
}
