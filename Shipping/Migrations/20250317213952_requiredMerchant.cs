using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shipping.Migrations
{
    /// <inheritdoc />
    public partial class requiredMerchant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Orders_OrderId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Products_Product_Id",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrderId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Product_Id",
                table: "Products",
                newName: "Order_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Product_Id",
                table: "Products",
                newName: "IX_Products_Order_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Orders_Order_Id",
                table: "Products",
                column: "Order_Id",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Orders_Order_Id",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Order_Id",
                table: "Products",
                newName: "Product_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Order_Id",
                table: "Products",
                newName: "IX_Products_Product_Id");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderId",
                table: "Products",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Orders_OrderId",
                table: "Products",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Products_Product_Id",
                table: "Products",
                column: "Product_Id",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
