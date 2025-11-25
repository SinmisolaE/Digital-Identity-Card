using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Issuer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEntityattribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResetPasswordtoken",
                table: "Users",
                newName: "ResetPasswordToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResetPasswordToken",
                table: "Users",
                newName: "ResetPasswordtoken");
        }
    }
}
