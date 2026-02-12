using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insurance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBrokerIdToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BrokerId",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Clients_BrokerId",
                table: "Clients",
                column: "BrokerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Brokers_BrokerId",
                table: "Clients",
                column: "BrokerId",
                principalTable: "Brokers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Brokers_BrokerId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_BrokerId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "BrokerId",
                table: "Clients");
        }
    }
}
