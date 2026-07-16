using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greetly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Occasions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occasions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Styles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    AgentPromptHint = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Styles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Greetings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OccasionId = table.Column<int>(type: "INTEGER", nullable: false),
                    StyleId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecipientName = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    ContentText = table.Column<string>(type: "TEXT", nullable: false),
                    DesignSvgUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Visibility = table.Column<int>(type: "INTEGER", nullable: false),
                    ViewCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Greetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Greetings_Occasions_OccasionId",
                        column: x => x.OccasionId,
                        principalTable: "Occasions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Greetings_Styles_StyleId",
                        column: x => x.StyleId,
                        principalTable: "Styles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Greetings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GreetingDrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GreetingId = table.Column<Guid>(type: "TEXT", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OccasionId = table.Column<int>(type: "INTEGER", nullable: false),
                    StyleId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecipientName = table.Column<string>(type: "TEXT", nullable: false),
                    RawUserText = table.Column<string>(type: "TEXT", nullable: false),
                    UserInstruction = table.Column<string>(type: "TEXT", nullable: true),
                    DraftContent = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreetingDrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GreetingDrafts_Greetings_GreetingId",
                        column: x => x.GreetingId,
                        principalTable: "Greetings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GreetingDrafts_Occasions_OccasionId",
                        column: x => x.OccasionId,
                        principalTable: "Occasions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GreetingDrafts_Styles_StyleId",
                        column: x => x.StyleId,
                        principalTable: "Styles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GreetingDrafts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GreetingFavorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GreetingId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreetingFavorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GreetingFavorites_Greetings_GreetingId",
                        column: x => x.GreetingId,
                        principalTable: "Greetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GreetingFavorites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GreetingDrafts_GreetingId",
                table: "GreetingDrafts",
                column: "GreetingId");

            migrationBuilder.CreateIndex(
                name: "IX_GreetingDrafts_OccasionId",
                table: "GreetingDrafts",
                column: "OccasionId");

            migrationBuilder.CreateIndex(
                name: "IX_GreetingDrafts_StyleId",
                table: "GreetingDrafts",
                column: "StyleId");

            migrationBuilder.CreateIndex(
                name: "IX_GreetingDrafts_UserId",
                table: "GreetingDrafts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GreetingFavorites_GreetingId",
                table: "GreetingFavorites",
                column: "GreetingId");

            migrationBuilder.CreateIndex(
                name: "IX_GreetingFavorites_UserId_GreetingId",
                table: "GreetingFavorites",
                columns: new[] { "UserId", "GreetingId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Greetings_OccasionId",
                table: "Greetings",
                column: "OccasionId");

            migrationBuilder.CreateIndex(
                name: "IX_Greetings_StyleId",
                table: "Greetings",
                column: "StyleId");

            migrationBuilder.CreateIndex(
                name: "IX_Greetings_UserId",
                table: "Greetings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GreetingDrafts");

            migrationBuilder.DropTable(
                name: "GreetingFavorites");

            migrationBuilder.DropTable(
                name: "Greetings");

            migrationBuilder.DropTable(
                name: "Occasions");

            migrationBuilder.DropTable(
                name: "Styles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
