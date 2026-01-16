using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace News_App.Migrations
{
    /// <inheritdoc />
    public partial class manytomanyrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_newspage_newspageID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_newspage_AspNetUsers_UserId",
                table: "newspage");

            migrationBuilder.DropIndex(
                name: "IX_newspage_UserId",
                table: "newspage");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_newspageID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "newspage");

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "newspage",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "NewspagePublishers",
                columns: table => new
                {
                    NewspageId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewspagePublishers", x => new { x.NewspageId, x.UserId });
                    table.ForeignKey(
                        name: "FK_NewspagePublishers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewspagePublishers_newspage_NewspageId",
                        column: x => x.NewspageId,
                        principalTable: "newspage",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_newspage_AdminId",
                table: "newspage",
                column: "AdminId",
                unique: true,
                filter: "[AdminId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NewspagePublishers_UserId",
                table: "NewspagePublishers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_newspage_AspNetUsers_AdminId",
                table: "newspage",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_newspage_AspNetUsers_AdminId",
                table: "newspage");

            migrationBuilder.DropTable(
                name: "NewspagePublishers");

            migrationBuilder.DropIndex(
                name: "IX_newspage_AdminId",
                table: "newspage");

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "newspage",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "newspage",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_newspage_UserId",
                table: "newspage",
                column: "UserId");

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
                name: "FK_newspage_AspNetUsers_UserId",
                table: "newspage",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
