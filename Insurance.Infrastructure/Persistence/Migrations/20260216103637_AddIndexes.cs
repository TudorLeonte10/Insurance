using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insurance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Buildings_City_Type",
                table: "Buildings",
                columns: new[] { "CityId", "Type" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Buildings_City_Type",
                table: "Buildings");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_City_Type",
                table: "Buildings",
                columns: new[] { "CityId", "Type" },
                unique: true);
        }
    }
}
