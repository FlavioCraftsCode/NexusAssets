using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusAssets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWarrantyToAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "WarrantyExpiration",
                table: "Assets",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarrantyExpiration",
                table: "Assets");
        }
    }
}
