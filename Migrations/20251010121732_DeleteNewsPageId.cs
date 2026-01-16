using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace News_App.Migrations
{
    /// <inheritdoc />
    public partial class DeleteNewsPageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "newspageID",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "newspageID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
