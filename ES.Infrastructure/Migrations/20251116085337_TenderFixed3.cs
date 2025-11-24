using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ES.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TenderFixed3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "Tenders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenders_MaterialId",
                table: "Tenders",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_Materials_MaterialId",
                table: "Tenders",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_Materials_MaterialId",
                table: "Tenders");

            migrationBuilder.DropIndex(
                name: "IX_Tenders_MaterialId",
                table: "Tenders");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "Tenders");
        }
    }
}
