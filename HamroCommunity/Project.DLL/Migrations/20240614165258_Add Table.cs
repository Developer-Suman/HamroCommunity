using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DLL.Migrations
{
    /// <inheritdoc />
    public partial class AddTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_certificates_Documents_DocumentsId",
                table: "certificates");

            migrationBuilder.DropTable(
                name: "CertificatesDocumentsImages");

            migrationBuilder.DropIndex(
                name: "IX_certificates_DocumentsId",
                table: "certificates");

            migrationBuilder.DropColumn(
                name: "DocumentsId",
                table: "certificates");

            migrationBuilder.AlterColumn<string>(
                name: "Grade",
                table: "certificates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "certificates",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Board",
                table: "certificates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CertificateDocuments",
                columns: table => new
                {
                    DocumentsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CertificateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateDocuments", x => new { x.DocumentsId, x.CertificateId });
                    table.ForeignKey(
                        name: "FK_CertificateDocuments_Documents_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificateDocuments_certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CertificateImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CertificateImgURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertificateId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateImages_certificates_Id",
                        column: x => x.Id,
                        principalTable: "certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificateDocuments_CertificateId",
                table: "CertificateDocuments",
                column: "CertificateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificateDocuments");

            migrationBuilder.DropTable(
                name: "CertificateImages");

            migrationBuilder.AlterColumn<string>(
                name: "Grade",
                table: "certificates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "certificates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Board",
                table: "certificates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DocumentsId",
                table: "certificates",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CertificatesDocumentsImages",
                columns: table => new
                {
                    CertificatesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CertificateId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificatesDocumentsImages", x => x.CertificatesId);
                    table.ForeignKey(
                        name: "FK_CertificatesDocumentsImages_certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_certificates_DocumentsId",
                table: "certificates",
                column: "DocumentsId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificatesDocumentsImages_CertificateId",
                table: "CertificatesDocumentsImages",
                column: "CertificateId");

            migrationBuilder.AddForeignKey(
                name: "FK_certificates_Documents_DocumentsId",
                table: "certificates",
                column: "DocumentsId",
                principalTable: "Documents",
                principalColumn: "Id");
        }
    }
}
