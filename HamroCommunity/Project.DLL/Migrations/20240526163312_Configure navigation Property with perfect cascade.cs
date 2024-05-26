using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DLL.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurenavigationPropertywithperfectcascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Certificate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Board = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificate_Documents_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Citizenship",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IssuedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuedDistrict = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VDCOrMunicipality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CitizenshipNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizenship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Citizenship_Documents_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Signature",
                columns: table => new
                {
                    SignatureId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SignatureURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentsId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signature", x => x.SignatureId);
                    table.ForeignKey(
                        name: "FK_Signature_Documents_SignatureId",
                        column: x => x.SignatureId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CertificatesDocumentsImages",
                columns: table => new
                {
                    CertificatesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentsImagesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificatesDocumentsImages", x => new { x.CertificatesId, x.DocumentsImagesId });
                    table.ForeignKey(
                        name: "FK_CertificatesDocumentsImages_Certificate_CertificatesId",
                        column: x => x.CertificatesId,
                        principalTable: "Certificate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificatesDocumentsImages_DocumentImages_DocumentsImagesId",
                        column: x => x.DocumentsImagesId,
                        principalTable: "DocumentImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_DocumentsId",
                table: "Certificate",
                column: "DocumentsId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificatesDocumentsImages_DocumentsImagesId",
                table: "CertificatesDocumentsImages",
                column: "DocumentsImagesId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizenship_DocumentsId",
                table: "Citizenship",
                column: "DocumentsId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificatesDocumentsImages");

            migrationBuilder.DropTable(
                name: "Citizenship");

            migrationBuilder.DropTable(
                name: "Signature");

            migrationBuilder.DropTable(
                name: "Certificate");

            migrationBuilder.DropTable(
                name: "DocumentImages");

            migrationBuilder.DropTable(
                name: "Documents");
        }
    }
}
