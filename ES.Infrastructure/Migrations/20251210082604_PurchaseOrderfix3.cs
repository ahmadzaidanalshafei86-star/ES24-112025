using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace ES.Infrastructure.Migrations
{
    public partial class PurchaseOrderfix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // PurchaseOrders table
            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                          .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 300, nullable: false),
                    Slug = table.Column<string>(maxLength: 300, nullable: false),
                    Code = table.Column<string>(maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    EnvelopeOpeningDate = table.Column<DateTime>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaKeywords = table.Column<string>(nullable: true),
                    MaterialId = table.Column<int>(nullable: true),
                    PurchaseOrderImageUrl = table.Column<string>(nullable: true),
                    PurchaseOrderImageAltName = table.Column<string>(nullable: true),
                    Numberofparticipatingcompanies = table.Column<string>(nullable: false),
                    Thenumberofcompaniesreferredto = table.Column<string>(nullable: false),
                    Publish = table.Column<bool>(nullable: false),
                    MoveToArchive = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    LanguageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                });

            // PurchaseOrdersFiles table
            migrationBuilder.CreateTable(
                name: "PurchaseOrdersFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                          .Annotation("SqlServer:Identity", "1, 1"),
                    AltName = table.Column<string>(maxLength: 255, nullable: false),
                    FileUrl = table.Column<string>(maxLength: 255, nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    PurchaseOrderId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrdersFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrdersFiles_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // PurchaseOrderMaterials table (junction table)
            migrationBuilder.CreateTable(
                name: "PurchaseOrderMaterials",
                columns: table => new
                {
                    PurchaseOrderId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderMaterials", x => new { x.PurchaseOrderId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_PurchaseOrderMaterials_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            // PurchaseOrderTranslates table
            migrationBuilder.CreateTable(
                name: "PurchaseOrderTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                          .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    PricesOffered = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaKeywords = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    PurchaseOrderId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderTranslates_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            // Indexes
            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_Code",
                table: "PurchaseOrders",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_LanguageId",
                table: "PurchaseOrders",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_MaterialId",
                table: "PurchaseOrders",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrdersFiles_PurchaseOrderId",
                table: "PurchaseOrdersFiles",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderMaterials_MaterialId",
                table: "PurchaseOrderMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderTranslates_PurchaseOrderId",
                table: "PurchaseOrderTranslates",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderTranslates_LanguageId",
                table: "PurchaseOrderTranslates",
                column: "LanguageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PurchaseOrderTranslates");
            migrationBuilder.DropTable(name: "PurchaseOrderMaterials");
            migrationBuilder.DropTable(name: "PurchaseOrdersFiles");
            migrationBuilder.DropTable(name: "PurchaseOrders");
        }
    }
}
