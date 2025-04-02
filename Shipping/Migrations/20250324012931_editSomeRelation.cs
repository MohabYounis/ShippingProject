using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shipping.Migrations
{
    /// <inheritdoc />
    public partial class editSomeRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingToVillages_Setting_Id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_WeightPricings_WeightPricing_Id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Setting_Id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_WeightPricing_Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DefaultPrice",
                table: "WeightPricings");

            migrationBuilder.DropColumn(
                name: "Setting_Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WeightPricing_Id",
                table: "Orders");

            migrationBuilder.AddColumn<decimal>(
                name: "CompanyRight",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryRight",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyRight",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryRight",
                table: "Orders");

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultPrice",
                table: "WeightPricings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Setting_Id",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeightPricing_Id",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Setting_Id",
                table: "Orders",
                column: "Setting_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WeightPricing_Id",
                table: "Orders",
                column: "WeightPricing_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingToVillages_Setting_Id",
                table: "Orders",
                column: "Setting_Id",
                principalTable: "ShippingToVillages",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_WeightPricings_WeightPricing_Id",
                table: "Orders",
                column: "WeightPricing_Id",
                principalTable: "WeightPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
