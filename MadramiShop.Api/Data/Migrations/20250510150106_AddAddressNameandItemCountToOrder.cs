using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MadramiShop.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressNameandItemCountToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalItems",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalItems",
                table: "Orders");
        }
    }
}
