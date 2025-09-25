using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageLevel = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TargetLanguage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUsedLanguageLevel = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LastUsedTargetLanguage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PreferredTranslationLanguage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SeenStoryIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SavedStoryIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Sentences",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    StoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartChar = table.Column<int>(type: "int", nullable: false),
                    EndChar = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sentences", x => new { x.Id, x.StoryId });
                    table.ForeignKey(
                        name: "FK_Sentences_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    StoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SentenceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDiscontinuous = table.Column<bool>(type: "bit", nullable: false),
                    Pieces = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => new { x.Id, x.StoryId });
                    table.ForeignKey(
                        name: "FK_Units_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Unit_Segments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitId = table.Column<string>(type: "varchar(64)", nullable: false),
                    StoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartChar = table.Column<int>(type: "int", nullable: false),
                    EndChar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit_Segments", x => new { x.Id, x.UnitId, x.StoryId });
                    table.ForeignKey(
                        name: "FK_Unit_Segments_Units_UnitId_StoryId",
                        columns: x => new { x.UnitId, x.StoryId },
                        principalTable: "Units",
                        principalColumns: new[] { "Id", "StoryId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sentences_StoryId",
                table: "Sentences",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_CreatedAt",
                table: "Stories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_LanguageLevel_TargetLanguage_CreatedAt",
                table: "Stories",
                columns: new[] { "LanguageLevel", "StoryLanguage", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Unit_Segments_UnitId_StoryId",
                table: "Unit_Segments",
                columns: new[] { "UnitId", "StoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_Units_StoryId",
                table: "Units",
                column: "StoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sentences");

            migrationBuilder.DropTable(
                name: "Unit_Segments");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Stories");
        }
    }
}
