using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finnisimo_Library_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialStockToBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InitialStock",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialStock",
                table: "Books");
        }
    }
}
