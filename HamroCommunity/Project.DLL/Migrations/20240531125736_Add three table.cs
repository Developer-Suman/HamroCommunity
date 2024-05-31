using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DLL.Migrations
{
    /// <inheritdoc />
    public partial class Addthreetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentURL",
                table: "Certificate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentURL",
                table: "Certificate",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
