using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BE.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsSampleFieldInStdInList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "StdIns");

            migrationBuilder.CreateTable(
                name: "StdIns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSample= table.Column<bool>(type: "boolean", nullable:false),
                    StdIn = table.Column<string>(type: "text", nullable: false),
                    ProblemId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StdIns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StdIns_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StdIns_ProblemId",
                table: "StdIns",
                column: "ProblemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StdIns");
        }
    }
}
