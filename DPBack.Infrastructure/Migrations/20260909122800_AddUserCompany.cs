using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nip",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Regon",
                table: "Customers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Nip",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Regon",
                table: "Customers");
        }
    }
}
