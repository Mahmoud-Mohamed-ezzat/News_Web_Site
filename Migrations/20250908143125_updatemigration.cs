using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace News_App.Migrations
{
    /// <inheritdoc />
    public partial class updatemigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_post_AspNetUsers_PublisherId",
                table: "post");

            migrationBuilder.AlterColumn<string>(
                name: "PublisherId",
                table: "post",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "newspage",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "newspageID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_newspageID",
                table: "AspNetUsers",
                column: "newspageID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_newspage_newspageID",
                table: "AspNetUsers",
                column: "newspageID",
                principalTable: "newspage",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_post_AspNetUsers_PublisherId",
                table: "post",
                column: "PublisherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_newspage_newspageID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_post_AspNetUsers_PublisherId",
                table: "post");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_newspageID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "newspageID",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "PublisherId",
                table: "post",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "newspage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_post_AspNetUsers_PublisherId",
                table: "post",
                column: "PublisherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
