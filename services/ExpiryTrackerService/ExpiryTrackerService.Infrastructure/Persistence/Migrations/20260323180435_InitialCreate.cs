using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpiryTrackerService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "expiry");

            migrationBuilder.CreateTable(
                name: "ExpiryAlerts",
                schema: "expiry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PharmacyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BatchNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DaysToExpiry = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpiryAlerts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpiryAlerts_BatchId_CreatedAt",
                schema: "expiry",
                table: "ExpiryAlerts",
                columns: new[] { "BatchId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ExpiryAlerts_PharmacyId",
                schema: "expiry",
                table: "ExpiryAlerts",
                column: "PharmacyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpiryAlerts",
                schema: "expiry");
        }
    }
}
