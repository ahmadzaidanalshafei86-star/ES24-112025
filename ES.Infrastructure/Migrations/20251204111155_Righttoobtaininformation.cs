using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ES.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Righttoobtaininformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "RighttoobtaininformationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorizationBookNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommercialRegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorizationLetterDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelegateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LetterFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrandFatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalIDCopyFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResidenceGovernorate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResidenceCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResidenceDistrict = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkplaceGovernorate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkplaceCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Employer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CellPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LandlineNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    POBox = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentificationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OtherDocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InformationPurpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OtherPurpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OtherDeliveryMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InformationTopic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalDocumentsFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Agreement = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RighttoobtaininformationRequests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RighttoobtaininformationRequests");

         
        }
    }
}
