using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shipping.Migrations
{
    /// <inheritdoc />
    public partial class deletePickupFromCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickupShipping",
                table: "Cities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PickupShipping",
                table: "Cities",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
