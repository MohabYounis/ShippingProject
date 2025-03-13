using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shipping.Migrations
{
    /// <inheritdoc />
    public partial class DbEdit1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentTypes_PaymentType_Id",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "SpecialShippingRates");

            migrationBuilder.DropColumn(
                name: "CostLevel",
                table: "ShippingTypes");

            migrationBuilder.DropColumn(
                name: "ClientCity",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ClientGovernment",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "Merchants");

            migrationBuilder.RenameColumn(
                name: "PaymentType_Id",
                table: "Orders",
                newName: "ShippingToVillage_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_PaymentType_Id",
                table: "Orders",
                newName: "IX_Orders_ShippingToVillage_Id");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WeightPricings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "SpecialPrice",
                table: "SpecialShippingRates",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "ShippingTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "City_Id",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Delivery_Id",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dovernment_Id",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Government",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BranchMerchants",
                columns: table => new
                {
                    Merchant_Id = table.Column<int>(type: "int", nullable: false),
                    Branch_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchMerchants", x => new { x.Merchant_Id, x.Branch_Id });
                    table.ForeignKey(
                        name: "FK_BranchMerchants_Branches_Branch_Id",
                        column: x => x.Branch_Id,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BranchMerchants_Merchants_Merchant_Id",
                        column: x => x.Merchant_Id,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryGovernments",
                columns: table => new
                {
                    Delivery_Id = table.Column<int>(type: "int", nullable: false),
                    Government_Id = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryGovernments", x => new { x.Delivery_Id, x.Government_Id });
                    table.ForeignKey(
                        name: "FK_DeliveryGovernments_Deliveries_Delivery_Id",
                        column: x => x.Delivery_Id,
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DeliveryGovernments_Governments_Government_Id",
                        column: x => x.Government_Id,
                        principalTable: "Governments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ShippingToVillages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingToVillages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_City_Id",
                table: "Orders",
                column: "City_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Delivery_Id",
                table: "Orders",
                column: "Delivery_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Dovernment_Id",
                table: "Orders",
                column: "Dovernment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BranchMerchants_Branch_Id",
                table: "BranchMerchants",
                column: "Branch_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryGovernments_Government_Id",
                table: "DeliveryGovernments",
                column: "Government_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Cities_City_Id",
                table: "Orders",
                column: "City_Id",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Deliveries_Delivery_Id",
                table: "Orders",
                column: "Delivery_Id",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Governments_Dovernment_Id",
                table: "Orders",
                column: "Dovernment_Id",
                principalTable: "Governments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingToVillages_ShippingToVillage_Id",
                table: "Orders",
                column: "ShippingToVillage_Id",
                principalTable: "ShippingToVillages",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Cities_City_Id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Deliveries_Delivery_Id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Governments_Dovernment_Id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingToVillages_ShippingToVillage_Id",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "BranchMerchants");

            migrationBuilder.DropTable(
                name: "DeliveryGovernments");

            migrationBuilder.DropTable(
                name: "ShippingToVillages");

            migrationBuilder.DropIndex(
                name: "IX_Orders_City_Id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Delivery_Id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Dovernment_Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WeightPricings");

            migrationBuilder.DropColumn(
                name: "SpecialPrice",
                table: "SpecialShippingRates");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "ShippingTypes");

            migrationBuilder.DropColumn(
                name: "City_Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Delivery_Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Dovernment_Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Government",
                table: "Merchants");

            migrationBuilder.RenameColumn(
                name: "ShippingToVillage_Id",
                table: "Orders",
                newName: "PaymentType_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ShippingToVillage_Id",
                table: "Orders",
                newName: "IX_Orders_PaymentType_Id");

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "SpecialShippingRates",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CostLevel",
                table: "ShippingTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClientCity",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientGovernment",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "Merchants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentTypes_PaymentType_Id",
                table: "Orders",
                column: "PaymentType_Id",
                principalTable: "PaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
