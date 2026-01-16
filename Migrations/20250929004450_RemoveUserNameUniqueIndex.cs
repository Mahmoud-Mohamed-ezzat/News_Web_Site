using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace News_App.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserNameUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing unique index
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            // Recreate it as non-unique (and filtered for non-null values, if desired)
            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "UserName",
                unique: false,
                filter: "[UserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert to unique index
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");
        }
    }
}
