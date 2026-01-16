using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace News_App.Migrations
{
    /// <inheritdoc />
    public partial class imagesofpost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "post",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "post");
        }
    }
}
