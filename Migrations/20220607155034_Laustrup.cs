using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Migrations
{
    public partial class Laustrup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Comments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d94516d1-b337-40ca-be36-be3cf63145f9", "AQAAAAEAACcQAAAAEPisOPHO9DdgKFqIw0BOFty68V8FZBETkQnYrL6UrIahKW0+8MELntNBhnNRwrq//A==", "a5706420-6046-4c7c-a076-eaa00080ada8" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: 1,
                column: "TimeStamp",
                value: new DateTime(2022, 6, 7, 17, 50, 33, 770, DateTimeKind.Local).AddTicks(3496));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Comments");

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
                value: new DateTime(2022, 6, 7, 16, 33, 16, 594, DateTimeKind.Local).AddTicks(5888));
        }
    }
}
