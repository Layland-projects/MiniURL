using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniURL.Core.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkDurationDays = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email_Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email_Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserLevels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "UserLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MiniUrls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Url_Scheme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url_Host = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url_Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url_Query = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiresOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiniUrls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiniUrls_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserLevels",
                columns: new[] { "Id", "CreatedAt", "Description", "LinkDurationDays", "Name", "UpdatedAt" },
                values: new object[] { new Guid("fc562af7-a171-4d4d-96b5-c1d2e9332c52"), new DateTimeOffset(new DateTime(2021, 10, 6, 9, 58, 10, 447, DateTimeKind.Unspecified).AddTicks(8668), new TimeSpan(0, 0, 0, 0, 0)), "The default user level", 1, "Guest", null });

            migrationBuilder.InsertData(
                table: "UserLevels",
                columns: new[] { "Id", "CreatedAt", "Description", "LinkDurationDays", "Name", "UpdatedAt" },
                values: new object[] { new Guid("03b1f7a1-bc2b-4280-b770-fec905edd202"), new DateTimeOffset(new DateTime(2021, 10, 6, 9, 58, 10, 448, DateTimeKind.Unspecified).AddTicks(7203), new TimeSpan(0, 0, 0, 0, 0)), "The user level for someone who has signed up", 5, "Regular", null });

            migrationBuilder.InsertData(
                table: "UserLevels",
                columns: new[] { "Id", "CreatedAt", "Description", "LinkDurationDays", "Name", "UpdatedAt" },
                values: new object[] { new Guid("507b4845-6d68-49dd-b2c5-1d4c6d41cbc6"), new DateTimeOffset(new DateTime(2021, 10, 6, 9, 58, 10, 448, DateTimeKind.Unspecified).AddTicks(7281), new TimeSpan(0, 0, 0, 0, 0)), "The user level for a paying customer", null, "Premium", null });

            migrationBuilder.CreateIndex(
                name: "IX_MiniUrls_Reference",
                table: "MiniUrls",
                column: "Reference",
                unique: true,
                filter: "[Reference] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MiniUrls_UserId",
                table: "MiniUrls",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLevels_Name",
                table: "UserLevels",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LevelId",
                table: "Users",
                column: "LevelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MiniUrls");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserLevels");
        }
    }
}
