using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Summary = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false),
                    IsHome = table.Column<bool>(type: "INTEGER", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogCategories",
                columns: table => new
                {
                    BlogId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategories", x => new { x.BlogId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_BlogCategories_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "Author", "Description", "ImageUrl", "IsHome", "Summary", "Title", "UploadDate" },
                values: new object[,]
                {
                    { 1, "Didem", "Life is so good. I'm so glad that i love life description", "1.jpg", true, "Life is so good. I'm so glad that i love life", "Life is good", new DateTime(2022, 12, 21, 22, 9, 22, 698, DateTimeKind.Local).AddTicks(71) },
                    { 2, "John", "Music is everything for any kind of living being description.", "2.jpg", true, "Music is everything for any kind of living being.", "Music is everything", new DateTime(2022, 12, 23, 22, 9, 22, 698, DateTimeKind.Local).AddTicks(86) },
                    { 3, "Emily", "We need to do sports for living a long and healthy life description", "3.jpg", false, "We need to do sports for living a long and healthy life", "Sport is crucial", new DateTime(2022, 12, 25, 22, 9, 22, 698, DateTimeKind.Local).AddTicks(88) },
                    { 4, "Marcus", "Living life is a kind of art even if we don't realize description", "4.jpg", true, "Living life is a kind of art even if we don't realize", "Living life", new DateTime(2022, 12, 27, 22, 9, 22, 698, DateTimeKind.Local).AddTicks(89) },
                    { 5, "Angelica", "We need to stay away from toxic positivity description", "5.jpg", false, "We need to stay away from toxic positivity.", "Being Positive", new DateTime(2022, 12, 29, 22, 9, 22, 698, DateTimeKind.Local).AddTicks(90) }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Life" },
                    { 2, "Music" },
                    { 3, "Sports" },
                    { 4, "Psychology" }
                });

            migrationBuilder.InsertData(
                table: "BlogCategories",
                columns: new[] { "BlogId", "CategoryId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 1 },
                    { 5, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategories_CategoryId",
                table: "BlogCategories",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogCategories");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
