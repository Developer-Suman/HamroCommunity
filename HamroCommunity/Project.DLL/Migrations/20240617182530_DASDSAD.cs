using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DLL.Migrations
{
    /// <inheritdoc />
    public partial class DASDSAD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CertificateImages_certificates_Id",
                table: "CertificateImages");

            migrationBuilder.AlterColumn<string>(
                name: "CertificateId",
                table: "CertificateImages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateImages_CertificateId",
                table: "CertificateImages",
                column: "CertificateId");

            migrationBuilder.AddForeignKey(
                name: "FK_CertificateImages_certificates_CertificateId",
                table: "CertificateImages",
                column: "CertificateId",
                principalTable: "certificates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CertificateImages_certificates_CertificateId",
                table: "CertificateImages");

            migrationBuilder.DropIndex(
                name: "IX_CertificateImages_CertificateId",
                table: "CertificateImages");

            migrationBuilder.AlterColumn<string>(
                name: "CertificateId",
                table: "CertificateImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_CertificateImages_certificates_Id",
                table: "CertificateImages",
                column: "Id",
                principalTable: "certificates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
