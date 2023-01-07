using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VadgerWorkspace.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReleaseV110 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Clients",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastOrder",
                table: "Clients",
                type: "TEXT",
                nullable: true);

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
                name: "IsActive",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "LastOrder",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Clients");
        }
    }
}
