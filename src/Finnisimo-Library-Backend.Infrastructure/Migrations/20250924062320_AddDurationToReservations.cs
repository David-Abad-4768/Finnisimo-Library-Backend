using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finnisimo_Library_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDurationToReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DesiredLoanDurationInDays",
                table: "Reservations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtensionOfferExpiresAt",
                table: "Loans",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtensionOfferedAt",
                table: "Loans",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DesiredLoanDurationInDays",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ExtensionOfferExpiresAt",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "ExtensionOfferedAt",
                table: "Loans");
        }
    }
}
