using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddJsLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Language",
                table: "MainMethodBodies",
                newName: "LanguageId");

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Name" },
                values: new object[] { 63, "JavaScript" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.RenameColumn(
                name: "LanguageId",
                table: "MainMethodBodies",
                newName: "Language");
        }
    }
}
