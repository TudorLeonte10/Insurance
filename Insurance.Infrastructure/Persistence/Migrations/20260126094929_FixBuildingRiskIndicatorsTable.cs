using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insurance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixBuildingRiskIndicatorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuildingRiskIndicators");

            migrationBuilder.CreateTable(
                name: "BuildingRiskIndicators",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BuildingId = table.Column<Guid>(nullable: false),
                    RiskIndicator = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingRiskIndicators", x => x.Id);

                    table.ForeignKey(
                        name: "FK_BuildingRiskIndicators_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuildingRiskIndicators_BuildingId_RiskIndicator",
                table: "BuildingRiskIndicators",
                columns: new[] { "BuildingId", "RiskIndicator" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuildingRiskIndicators");

            migrationBuilder.CreateTable(
                name: "BuildingRiskIndicators",
                columns: table => new
                {
                    BuildingId = table.Column<Guid>(nullable: false),
                    RiskIndicator = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_BuildingRiskIndicators",
                        x => new { x.BuildingId, x.RiskIndicator });

                    table.ForeignKey(
                        name: "FK_BuildingRiskIndicators_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
