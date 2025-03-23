using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shipping.Migrations
{
    /// <inheritdoc />
    public partial class orderedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Governments_Dovernment_Id",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "TotalWeight",
                table: "Orders",
                newName: "OrderTotalWeight");

            migrationBuilder.RenameColumn(
                name: "Dovernment_Id",
                table: "Orders",
                newName: "Government_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_Dovernment_Id",
                table: "Orders",
                newName: "IX_Orders_Government_Id");

            migrationBuilder.AddColumn<int>(
                name: "Branch_Id",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "OrderCost",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Branches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Branch_Id",
                table: "Orders",
                column: "Branch_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_BranchId",
                table: "Branches",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Branches_BranchId",
                table: "Branches",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Branches_Branch_Id",
                table: "Orders",
                column: "Branch_Id",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Governments_Government_Id",
                table: "Orders",
                column: "Government_Id",
                principalTable: "Governments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Branches_BranchId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Branches_Branch_Id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Governments_Government_Id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Branch_Id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Branches_BranchId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Branch_Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderCost",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "ProfileImagePath",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "OrderTotalWeight",
                table: "Orders",
                newName: "TotalWeight");

            migrationBuilder.RenameColumn(
                name: "Government_Id",
                table: "Orders",
                newName: "Dovernment_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_Government_Id",
                table: "Orders",
                newName: "IX_Orders_Dovernment_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Governments_Dovernment_Id",
                table: "Orders",
                column: "Dovernment_Id",
                principalTable: "Governments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
