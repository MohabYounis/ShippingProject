using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shipping.Migrations
{
    /// <inheritdoc />
    public partial class editUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
            name: "UX_PasswordResetOtps_OneActivePerUser",
            table: "PasswordResetOtps");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PasswordResetOtps",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
            // 3️⃣ رجّع الـ Index تاني
            migrationBuilder.CreateIndex(
                name: "UX_PasswordResetOtps_OneActivePerUser",
                table: "PasswordResetOtps",
                column: "UserId",
                unique: true,
                filter: "[IsUsed] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropIndex(
            name: "UX_PasswordResetOtps_OneActivePerUser",
            table: "PasswordResetOtps");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "PasswordResetOtps",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.CreateIndex(
                   name: "UX_PasswordResetOtps_OneActivePerUser",
                   table: "PasswordResetOtps",
                   column: "UserId",
                   unique: true,
                   filter: "[IsUsed] = 0");
        }
    }
}
