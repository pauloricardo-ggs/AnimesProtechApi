using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Seed_Admin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a16dfc59-09b3-48bc-9856-9a9b684f553f"),
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash" },
                values: new object[] { "f8e5df74-87f9-413d-b100-2c6d38a0cbd1", "CONTACT@PAULORICARDO.DEV", "CONTACT@PAULORICARDO.DEV", "AQAAAAIAAYagAAAAEJ7k4C9Xxxs6VMwgN9KTkD2p0ORUrmzYNw/4Psos+LUAzw8Yb1KVxIdSclJFqDOg8A==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a16dfc59-09b3-48bc-9856-9a9b684f553f"),
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash" },
                values: new object[] { "114dbf27-89df-4d59-9123-6688294d081f", null, null, "AQAAAAIAAYagAAAAEJHQGlVYkNjOkAl1BjR7aZreDxJfoGjRQ3Em22n6P9VjWEGwhldhXnCebmhrAPt7Mw==" });
        }
    }
}
