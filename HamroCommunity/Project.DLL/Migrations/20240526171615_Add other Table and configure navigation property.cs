using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DLL.Migrations
{
    /// <inheritdoc />
    public partial class AddotherTableandconfigurenavigationproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentsImagesId",
                table: "Citizenship",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Citizenship_DocumentsImagesId",
                table: "Citizenship",
                column: "DocumentsImagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Citizenship_DocumentImages_DocumentsImagesId",
                table: "Citizenship",
                column: "DocumentsImagesId",
                principalTable: "DocumentImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citizenship_DocumentImages_DocumentsImagesId",
                table: "Citizenship");

            migrationBuilder.DropIndex(
                name: "IX_Citizenship_DocumentsImagesId",
                table: "Citizenship");

            migrationBuilder.DropColumn(
                name: "DocumentsImagesId",
                table: "Citizenship");
        }
    }
}
