using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RedditClone.Data.Migrations
{
    public partial class AddedSubredditCreatedDateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Subreddits",
                nullable: false,
                defaultValue: new DateTime(2019, 4, 16, 18, 45, 0, 473, DateTimeKind.Utc).AddTicks(8509));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostDate",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2019, 4, 16, 18, 45, 0, 435, DateTimeKind.Utc).AddTicks(2875),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 4, 7, 19, 54, 42, 106, DateTimeKind.Utc).AddTicks(3813));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostDate",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2019, 4, 16, 18, 45, 0, 456, DateTimeKind.Utc).AddTicks(7539),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 4, 7, 19, 54, 42, 141, DateTimeKind.Utc).AddTicks(350));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Subreddits");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostDate",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2019, 4, 7, 19, 54, 42, 106, DateTimeKind.Utc).AddTicks(3813),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 4, 16, 18, 45, 0, 435, DateTimeKind.Utc).AddTicks(2875));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostDate",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2019, 4, 7, 19, 54, 42, 141, DateTimeKind.Utc).AddTicks(350),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 4, 16, 18, 45, 0, 456, DateTimeKind.Utc).AddTicks(7539));
        }
    }
}
