using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Add_Anime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animes",
                columns: table => new
                {
                    AnimeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Director = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animes", x => x.AnimeId);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a16dfc59-09b3-48bc-9856-9a9b684f553f"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "036df4b2-0fd5-4547-8029-876eb622a3fb", "AQAAAAIAAYagAAAAEDvRXk0nZDdiYPLO1xjFPc7ixLpE7P0etKrNqb0dwc/lFNUbvtOVuFqz0Fm7nQX3uA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a16dfc59-09b3-48bc-9856-9a9b684f553f"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f8e5df74-87f9-413d-b100-2c6d38a0cbd1", "AQAAAAIAAYagAAAAEJ7k4C9Xxxs6VMwgN9KTkD2p0ORUrmzYNw/4Psos+LUAzw8Yb1KVxIdSclJFqDOg8A==" });
        }
    }
}
