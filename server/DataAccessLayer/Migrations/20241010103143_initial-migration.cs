using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => new { x.Name, x.NormalizedName });
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CustomUrl = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Thumbnail = table.Column<string>(type: "text", nullable: false),
                    SubscriberCount = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    VideoCount = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    LastUpdatedAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChannelCategories",
                columns: table => new
                {
                    ChannelId = table.Column<string>(type: "text", nullable: false),
                    CategoryName = table.Column<string>(type: "text", nullable: false),
                    CategoryNormalizedName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelCategories", x => new { x.ChannelId, x.CategoryName, x.CategoryNormalizedName });
                    table.ForeignKey(
                        name: "FK_ChannelCategories_Categories_CategoryName_CategoryNormalize~",
                        columns: x => new { x.CategoryName, x.CategoryNormalizedName },
                        principalTable: "Categories",
                        principalColumns: new[] { "Name", "NormalizedName" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelCategories_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_NormalizedName",
                table: "Categories",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelCategories_CategoryName_CategoryNormalizedName",
                table: "ChannelCategories",
                columns: new[] { "CategoryName", "CategoryNormalizedName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelCategories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Channels");
        }
    }
}
