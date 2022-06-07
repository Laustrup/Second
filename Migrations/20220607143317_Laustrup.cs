using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Migrations
{
    public partial class Laustrup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Comments",
                newName: "CommentId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ab304d6d-df9f-4e5f-9a6a-e5dce4a6e9ea", "AQAAAAEAACcQAAAAEO9b21DukwKpqwZnc56xanzruRHeLZBPAU+aMO4f3IAYeXNSw4tlCAmo2Xa3iItK2Q==", "155dfc3a-697b-441a-984b-8d4fd2fb6e55" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "TimeStamp",
                value: new DateTime(2022, 6, 7, 16, 33, 16, 843, DateTimeKind.Local).AddTicks(2550));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comments",
                newName: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2813afeb-c6f0-41f1-aee6-5b1bac50b69f", "AQAAAAEAACcQAAAAEMBZbI1s5fjOvPwCy1HXDQYDB+sdxfNk372jj5Sr1/XSJeBDyd9gt8rClNZeVqzPxw==", "b6cf65e8-e340-40fa-9771-344364748a03" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "TimeStamp",
                value: new DateTime(2022, 6, 7, 16, 7, 18, 715, DateTimeKind.Local).AddTicks(78));
        }
    }
}
