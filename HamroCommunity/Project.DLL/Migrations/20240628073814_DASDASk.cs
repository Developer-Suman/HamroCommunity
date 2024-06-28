using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DLL.Migrations
{
    /// <inheritdoc />
    public partial class DASDASk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Nijamatis_NijamatiId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_NijamatiId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "NijamatiId",
                table: "Documents");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentsId",
                table: "Nijamatis",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Nijamatis_DocumentsId",
                table: "Nijamatis",
                column: "DocumentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nijamatis_Documents_DocumentsId",
                table: "Nijamatis",
                column: "DocumentsId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nijamatis_Documents_DocumentsId",
                table: "Nijamatis");

            migrationBuilder.DropIndex(
                name: "IX_Nijamatis_DocumentsId",
                table: "Nijamatis");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentsId",
                table: "Nijamatis",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "NijamatiId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_NijamatiId",
                table: "Documents",
                column: "NijamatiId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Nijamatis_NijamatiId",
                table: "Documents",
                column: "NijamatiId",
                principalTable: "Nijamatis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
