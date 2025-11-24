using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ES.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Branches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchesTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchesTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchesTranslate_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchesTranslate_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Hangars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hangars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hangars_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Refrigators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refrigators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refrigators_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HangarsTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HangarId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangarsTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HangarsTranslate_Hangars_HangarId",
                        column: x => x.HangarId,
                        principalTable: "Hangars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HangarsTranslate_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefrigatorsTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefrigatorId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefrigatorsTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefrigatorsTranslate_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RefrigatorsTranslate_Refrigators_RefrigatorId",
                        column: x => x.RefrigatorId,
                        principalTable: "Refrigators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_LanguageId",
                table: "Branches",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Slug",
                table: "Branches",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BranchesTranslate_BranchId",
                table: "BranchesTranslate",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchesTranslate_LanguageId",
                table: "BranchesTranslate",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Hangars_BranchId",
                table: "Hangars",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_HangarsTranslate_HangarId",
                table: "HangarsTranslate",
                column: "HangarId");

            migrationBuilder.CreateIndex(
                name: "IX_HangarsTranslate_LanguageId",
                table: "HangarsTranslate",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Refrigators_BranchId",
                table: "Refrigators",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_RefrigatorsTranslate_LanguageId",
                table: "RefrigatorsTranslate",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_RefrigatorsTranslate_RefrigatorId",
                table: "RefrigatorsTranslate",
                column: "RefrigatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchesTranslate");

            migrationBuilder.DropTable(
                name: "HangarsTranslate");

            migrationBuilder.DropTable(
                name: "RefrigatorsTranslate");

            migrationBuilder.DropTable(
                name: "Hangars");

            migrationBuilder.DropTable(
                name: "Refrigators");

            migrationBuilder.DropTable(
                name: "Branches");
        }
    }
}
