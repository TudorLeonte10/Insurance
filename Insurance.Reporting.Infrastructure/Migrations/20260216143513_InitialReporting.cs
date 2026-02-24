using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insurance.Reporting.Infrastructure.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class InitialReporting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PolicyReportAggregates",
                columns: table => new
                {
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    County = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrokerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BuildingType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinalPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalPremiumInBase = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyReportAggregates", x => x.PolicyId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PolicyReportAggregates_Country",
                table: "PolicyReportAggregates",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyReportAggregates_CreatedAt",
                table: "PolicyReportAggregates",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyReportAggregates_Currency",
                table: "PolicyReportAggregates",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyReportAggregates_Status",
                table: "PolicyReportAggregates",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolicyReportAggregates");
        }
    }
}
