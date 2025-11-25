using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Issuer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOutBoxMessagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_outBoxMessages",
                table: "outBoxMessages");

            migrationBuilder.RenameTable(
                name: "outBoxMessages",
                newName: "OutBoxMessages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TokenExpiry",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Creaded_At",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Error",
                table: "OutBoxMessages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutBoxMessages",
                table: "OutBoxMessages",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutBoxMessages",
                table: "OutBoxMessages");

            migrationBuilder.RenameTable(
                name: "OutBoxMessages",
                newName: "outBoxMessages");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "TokenExpiry",
                table: "Users",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Creaded_At",
                table: "Users",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Error",
                table: "outBoxMessages",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_outBoxMessages",
                table: "outBoxMessages",
                column: "Id");
        }
    }
}
