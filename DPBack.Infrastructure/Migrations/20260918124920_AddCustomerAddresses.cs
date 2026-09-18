using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatusHistories_Orders_OrderEntityId",
                table: "OrderStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatusHistories_OrderEntityId",
                table: "OrderStatusHistories");

            migrationBuilder.DropColumn(
                name: "OrderEntityId",
                table: "OrderStatusHistories");

            migrationBuilder.CreateTable(
                name: "CustomerAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Street = table.Column<string>(type: "text", nullable: true),
                    BuildingNumber = table.Column<string>(type: "text", nullable: true),
                    ApartmentNumber = table.Column<string>(type: "text", nullable: true),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Options = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAddresses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddresses_CustomerId",
                table: "CustomerAddresses",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerAddresses");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderEntityId",
                table: "OrderStatusHistories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistories_OrderEntityId",
                table: "OrderStatusHistories",
                column: "OrderEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatusHistories_Orders_OrderEntityId",
                table: "OrderStatusHistories",
                column: "OrderEntityId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
