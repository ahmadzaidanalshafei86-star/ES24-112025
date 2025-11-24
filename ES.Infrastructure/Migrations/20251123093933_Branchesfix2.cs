using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ES.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Branchesfix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "RefrigatorsTranslate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "RefrigatorsTranslate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Refrigators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Refrigators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "HangarsTranslate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "HangarsTranslate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Hangars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Hangars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "RefrigatorsTranslate");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "RefrigatorsTranslate");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Refrigators");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Refrigators");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "HangarsTranslate");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "HangarsTranslate");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Hangars");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Hangars");
        }
    }
}
