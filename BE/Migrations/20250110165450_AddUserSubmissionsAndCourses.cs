using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BE.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSubmissionsAndCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequiredPercentageToPass",
                table: "Problems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CourseId",
                table: "Problems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSubmissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceCode = table.Column<string>(type: "text", nullable: false),
                    IsPassing = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ProblemId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSubmissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSubmissions_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSubmissions_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestCases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    Stdout = table.Column<string>(type: "text", nullable: true),
                    CompileOutput = table.Column<string>(type: "text", nullable: true),
                    Stderr = table.Column<string>(type: "text", nullable: true),
                    ExpectedOutput = table.Column<string>(type: "text", nullable: true),
                    HiddenExpectedOutput = table.Column<string>(type: "text", nullable: true),
                    UserSubmissionModelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCases_UserSubmissions_UserSubmissionModelId",
                        column: x => x.UserSubmissionModelId,
                        principalTable: "UserSubmissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TestCaseStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResultId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TestCaseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCaseStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCaseStatuses_TestCases_TestCaseId",
                        column: x => x.TestCaseId,
                        principalTable: "TestCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Problems_CourseId",
                table: "Problems",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TestCases_UserSubmissionModelId",
                table: "TestCases",
                column: "UserSubmissionModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TestCaseStatuses_TestCaseId",
                table: "TestCaseStatuses",
                column: "TestCaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSubmissions_LanguageId",
                table: "UserSubmissions",
                column: "LanguageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSubmissions_ProblemId",
                table: "UserSubmissions",
                column: "ProblemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSubmissions_UserId",
                table: "UserSubmissions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_Courses_CourseId",
                table: "Problems",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Courses_CourseId",
                table: "Problems");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "TestCaseStatuses");

            migrationBuilder.DropTable(
                name: "TestCases");

            migrationBuilder.DropTable(
                name: "UserSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_Problems_CourseId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "RequiredPercentageToPass",
                table: "Problems");
        }
    }
}
