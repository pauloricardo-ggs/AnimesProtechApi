using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Add_Request_Logger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("56fb0e8e-e16c-494b-b065-4c9d6a91dec2"));

            migrationBuilder.CreateTable(
                name: "RequestType",
                columns: table => new
                {
                    RequestTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Path = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestType", x => x.RequestTypeId);
                });

            migrationBuilder.CreateTable(
                name: "requests",
                columns: table => new
                {
                    request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    url = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    request_data = table.Column<string>(type: "text", nullable: false),
                    response_data = table.Column<string>(type: "text", nullable: false),
                    log_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    http_code = table.Column<int>(type: "integer", nullable: true),
                    request_type_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requests", x => x.request_id);
                    table.ForeignKey(
                        name: "FK_requests_RequestType_request_type_id",
                        column: x => x.request_type_id,
                        principalTable: "RequestType",
                        principalColumn: "RequestTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RequestType",
                columns: new[] { "RequestTypeId", "Name", "Path" },
                values: new object[,]
                {
                    { new Guid("420b212b-7506-4776-b810-401f41c31465"), "Delete Anime", "Application.Handlers.AnimeCommandHandler.Handle(DeleteAnimeCommand)" },
                    { new Guid("648a145c-908c-4dec-a2cf-7a1474a2ca74"), "Create Anime", "Application.Handlers.AnimeCommandHandler.Handle(CreateAnimeCommand)" },
                    { new Guid("bf8ac0d7-9dfb-4890-876b-e39fc36739e7"), "Update Anime", "Application.Handlers.AnimeCommandHandler.Handle(UpdateAnimeCommand)" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a16dfc59-09b3-48bc-9856-9a9b684f553f"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e5797d2c-3f4f-4e62-b1e4-b33f8a26b401", "AQAAAAIAAYagAAAAEHHEojgyxbojsl1cECHVVgiJDW5PJWjE3OCi5lOsNBlaRB3B2pjIXdSEswcK+QNJVQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_requests_request_type_id",
                table: "requests",
                column: "request_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "requests");

            migrationBuilder.DropTable(
                name: "RequestType");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("56fb0e8e-e16c-494b-b065-4c9d6a91dec2"), null, "user", "USER" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a16dfc59-09b3-48bc-9856-9a9b684f553f"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "036df4b2-0fd5-4547-8029-876eb622a3fb", "AQAAAAIAAYagAAAAEDvRXk0nZDdiYPLO1xjFPc7ixLpE7P0etKrNqb0dwc/lFNUbvtOVuFqz0Fm7nQX3uA==" });
        }
    }
}
