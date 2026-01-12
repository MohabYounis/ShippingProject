using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shipping.Migrations
{
    /// <inheritdoc />
    public partial class resetPasswordOtp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE UNIQUE INDEX UX_PasswordResetOtps_OneActivePerUser
            ON dbo.PasswordResetOtps(UserId)
            WHERE IsUsed = 0
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP INDEX UX_PasswordResetOtps_OneActivePerUser
            ON dbo.PasswordResetOtps
            ");
        }
    }
}
