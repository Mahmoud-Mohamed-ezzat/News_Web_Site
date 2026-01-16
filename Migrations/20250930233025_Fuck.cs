using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace News_App.Migrations
{
    /// <inheritdoc />
    public partial class Fuck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing unique index on NormalizedUserName
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            // Recreate as non-unique (no filter needed if you want full duplicates)
            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: false);  // No filter to allow nulls/duplicates freely
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert to unique
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");  // Default filter for SQL Server
        }
    }
}
