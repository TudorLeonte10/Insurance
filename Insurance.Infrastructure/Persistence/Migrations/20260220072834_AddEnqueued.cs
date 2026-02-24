using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Insurance.Infrastructure.Persistence.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class AddEnqueued : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enqueued",
                table: "OutboxEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enqueued",
                table: "OutboxEvents");
        }
    }
}
