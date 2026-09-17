using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageToHistoryElement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "OrderStatusHistories",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorLogin",
                table: "OrderStatusHistories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "OrderStatusHistories",
                type: "text",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatusHistories_Orders_OrderEntityId",
                table: "OrderStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatusHistories_OrderEntityId",
                table: "OrderStatusHistories");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "OrderStatusHistories");

            migrationBuilder.DropColumn(
                name: "OrderEntityId",
                table: "OrderStatusHistories");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "OrderStatusHistories",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorLogin",
                table: "OrderStatusHistories",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
