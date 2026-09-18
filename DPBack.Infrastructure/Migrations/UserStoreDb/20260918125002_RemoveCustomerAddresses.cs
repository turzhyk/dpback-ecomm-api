using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPBack.Infrastructure.Migrations.UserStoreDb
{
    /// <inheritdoc />
    public partial class RemoveCustomerAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adresses_Users_UserEntityId",
                table: "Adresses");

            migrationBuilder.DropIndex(
                name: "IX_Adresses_UserEntityId",
                table: "Adresses");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Adresses");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Adresses",
                newName: "CustomerId");

            migrationBuilder.CreateTable(
                name: "CustomerEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Nip = table.Column<string>(type: "text", nullable: true),
                    Regon = table.Column<string>(type: "text", nullable: true),
                    CompanyName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adresses_CustomerId",
                table: "Adresses",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adresses_CustomerEntity_CustomerId",
                table: "Adresses",
                column: "CustomerId",
                principalTable: "CustomerEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adresses_CustomerEntity_CustomerId",
                table: "Adresses");

            migrationBuilder.DropTable(
                name: "CustomerEntity");

            migrationBuilder.DropIndex(
                name: "IX_Adresses_CustomerId",
                table: "Adresses");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Adresses",
                newName: "UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityId",
                table: "Adresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adresses_UserEntityId",
                table: "Adresses",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adresses_Users_UserEntityId",
                table: "Adresses",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
