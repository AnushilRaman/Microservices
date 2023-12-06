using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservices.Services.OrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenamingthecolumnCarttoOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CartTotal",
                table: "OrderHeaders",
                newName: "OrderTotal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderTotal",
                table: "OrderHeaders",
                newName: "CartTotal");
        }
    }
}
