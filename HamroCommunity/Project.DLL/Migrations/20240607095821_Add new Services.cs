using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DLL.Migrations
{
    /// <inheritdoc />
    public partial class AddnewServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_Documents_DocumentsId",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_CertificatesDocumentsImages_Certificate_CertificatesId",
                table: "CertificatesDocumentsImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Signature_Documents_SignatureId",
                table: "Signature");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Signature",
                table: "Signature");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Certificate",
                table: "Certificate");

            migrationBuilder.RenameTable(
                name: "Signature",
                newName: "signatures");

            migrationBuilder.RenameTable(
                name: "Certificate",
                newName: "certificates");

            migrationBuilder.RenameIndex(
                name: "IX_Certificate_DocumentsId",
                table: "certificates",
                newName: "IX_certificates_DocumentsId");

            migrationBuilder.AddColumn<string>(
                name: "CreatedAt",
                table: "signatures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_signatures",
                table: "signatures",
                column: "SignatureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_certificates",
                table: "certificates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_certificates_Documents_DocumentsId",
                table: "certificates",
                column: "DocumentsId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CertificatesDocumentsImages_certificates_CertificatesId",
                table: "CertificatesDocumentsImages",
                column: "CertificatesId",
                principalTable: "certificates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_signatures_Documents_SignatureId",
                table: "signatures",
                column: "SignatureId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_certificates_Documents_DocumentsId",
                table: "certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_CertificatesDocumentsImages_certificates_CertificatesId",
                table: "CertificatesDocumentsImages");

            migrationBuilder.DropForeignKey(
                name: "FK_signatures_Documents_SignatureId",
                table: "signatures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_signatures",
                table: "signatures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_certificates",
                table: "certificates");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "signatures");

            migrationBuilder.RenameTable(
                name: "signatures",
                newName: "Signature");

            migrationBuilder.RenameTable(
                name: "certificates",
                newName: "Certificate");

            migrationBuilder.RenameIndex(
                name: "IX_certificates_DocumentsId",
                table: "Certificate",
                newName: "IX_Certificate_DocumentsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Signature",
                table: "Signature",
                column: "SignatureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Certificate",
                table: "Certificate",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_Documents_DocumentsId",
                table: "Certificate",
                column: "DocumentsId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CertificatesDocumentsImages_Certificate_CertificatesId",
                table: "CertificatesDocumentsImages",
                column: "CertificatesId",
                principalTable: "Certificate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Signature_Documents_SignatureId",
                table: "Signature",
                column: "SignatureId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
