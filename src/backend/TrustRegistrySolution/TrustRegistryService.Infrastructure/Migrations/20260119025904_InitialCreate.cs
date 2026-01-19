using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrustRegistryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrustRegistries",
                columns: table => new
                {
                    IssuerId = table.Column<string>(type: "text", nullable: false),
                    PublicKey = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrustRegistries", x => x.IssuerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrustRegistries_IssuerId",
                table: "TrustRegistries",
                column: "IssuerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrustRegistries");
        }
    }
}
