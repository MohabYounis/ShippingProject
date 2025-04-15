using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shipping.Migrations
{
    /// <inheritdoc />
    public partial class lastchnange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_Branch_Id",
                table: "Orders",
                column: "Branch_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Branches_Branch_Id",
                table: "Orders",
                column: "Branch_Id",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Branches_Branch_Id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Branch_Id",
                table: "Orders");
        }
    }
}
