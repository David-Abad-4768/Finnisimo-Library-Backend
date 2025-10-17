using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finnisimo_Library_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReadingLogEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReadingLogEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartPage = table.Column<int>(type: "integer", nullable: false),
                    EndPage = table.Column<int>(type: "integer", nullable: false),
                    PagesRead = table.Column<int>(type: "integer", nullable: false),
                    DateLogged = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingLogEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadingLogEntries_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReadingLogEntries_DateLogged",
                table: "ReadingLogEntries",
                column: "DateLogged");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingLogEntries_LoanId",
                table: "ReadingLogEntries",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingLogEntries_UserId",
                table: "ReadingLogEntries",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReadingLogEntries");
        }
    }
}
