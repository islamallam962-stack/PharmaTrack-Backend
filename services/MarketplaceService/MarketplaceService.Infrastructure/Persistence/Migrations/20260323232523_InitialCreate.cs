using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketplaceService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "marketplace");

            migrationBuilder.CreateTable(
                name: "Listings",
                schema: "marketplace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerPharmacyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BatchNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    QuantityAvailable = table.Column<int>(type: "integer", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountedPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DaysToExpiry = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                schema: "marketplace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BuyerPharmacyId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    QuantityNeeded = table.Column<int>(type: "integer", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    MatchedListingId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_BatchId",
                schema: "marketplace",
                table: "Listings",
                column: "BatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listings_ExpiryDate",
                schema: "marketplace",
                table: "Listings",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_ProductName",
                schema: "marketplace",
                table: "Listings",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_Status",
                schema: "marketplace",
                table: "Listings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_BuyerPharmacyId",
                schema: "marketplace",
                table: "Requests",
                column: "BuyerPharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ProductName",
                schema: "marketplace",
                table: "Requests",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_Status",
                schema: "marketplace",
                table: "Requests",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Listings",
                schema: "marketplace");

            migrationBuilder.DropTable(
                name: "Requests",
                schema: "marketplace");
        }
    }
}
