using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shipping.Migrations
{
    /// <inheritdoc />
    public partial class rejectedOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RejectReason_RejectReason_ID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RejectReason_ID",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "RejectedOrders",
                columns: table => new
                {
                    Order_Id = table.Column<int>(type: "int", nullable: false),
                    RejectReason_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RejectedOrders", x => new { x.Order_Id, x.RejectReason_Id });
                    table.ForeignKey(
                        name: "FK_RejectedOrders_Orders_Order_Id",
                        column: x => x.Order_Id,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RejectedOrders_RejectReason_RejectReason_Id",
                        column: x => x.RejectReason_Id,
                        principalTable: "RejectReason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RejectedOrders_RejectReason_Id",
                table: "RejectedOrders",
                column: "RejectReason_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RejectedOrders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RejectReason_ID",
                table: "Orders",
                column: "RejectReason_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RejectReason_RejectReason_ID",
                table: "Orders",
                column: "RejectReason_ID",
                principalTable: "RejectReason",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
