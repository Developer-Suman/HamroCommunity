using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DLL.Migrations
{
    /// <inheritdoc />
    public partial class Chanegssomeonmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SignatureId",
                table: "Signatures",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "CitizenshipId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CitizenshipId",
                table: "Documents",
                column: "CitizenshipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Citizenships_CitizenshipId",
                table: "Documents",
                column: "CitizenshipId",
                principalTable: "Citizenships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Citizenships_CitizenshipId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_CitizenshipId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "CitizenshipId",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Signatures",
                newName: "SignatureId");
        }
    }
}
