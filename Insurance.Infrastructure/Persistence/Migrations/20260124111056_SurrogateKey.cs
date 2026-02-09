using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Insurance.Infrastructure.Persistence.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class SurrogateKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ Add Id as nullable
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "BuildingRiskIndicators",
                type: "uniqueidentifier",
                nullable: true);

            // 2️⃣ Fill existing rows
            migrationBuilder.Sql(
                @"UPDATE BuildingRiskIndicators
          SET Id = NEWID()");

            // 3️⃣ Make Id NOT NULL
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "BuildingRiskIndicators",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            // 4️⃣ Drop old composite PK
            migrationBuilder.DropPrimaryKey(
                name: "PK_BuildingRiskIndicators",
                table: "BuildingRiskIndicators");

            // 5️⃣ Add new PK
            migrationBuilder.AddPrimaryKey(
                name: "PK_BuildingRiskIndicators",
                table: "BuildingRiskIndicators",
                column: "Id");

            // 6️⃣ Add unique index for business rule
            migrationBuilder.CreateIndex(
                name: "IX_BuildingRiskIndicators_BuildingId_RiskIndicator",
                table: "BuildingRiskIndicators",
                columns: new[] { "BuildingId", "RiskIndicator" },
                unique: true);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ Drop unique index
            migrationBuilder.DropIndex(
                name: "IX_BuildingRiskIndicators_BuildingId_RiskIndicator",
                table: "BuildingRiskIndicators");

            // 2️⃣ Drop PK on Id
            migrationBuilder.DropPrimaryKey(
                name: "PK_BuildingRiskIndicators",
                table: "BuildingRiskIndicators");

            // 3️⃣ Recreate old composite PK
            migrationBuilder.AddPrimaryKey(
                name: "PK_BuildingRiskIndicators",
                table: "BuildingRiskIndicators",
                columns: new[] { "BuildingId", "RiskIndicator" });

            // 4️⃣ Drop Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "BuildingRiskIndicators");
        }
    }
}

