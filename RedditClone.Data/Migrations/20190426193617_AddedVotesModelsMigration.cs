using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RedditClone.Data.Migrations
{
    public partial class AddedVotesModelsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Subreddits",
                nullable: false,
                defaultValue: new DateTime(2019, 4, 26, 19, 36, 15, 406, DateTimeKind.Utc).AddTicks(5269),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 4, 16, 18, 45, 0, 473, DateTimeKind.Utc).AddTicks(8509));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostDate",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2019, 4, 26, 19, 36, 15, 255, DateTimeKind.Utc).AddTicks(8788),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 4, 16, 18, 45, 0, 435, DateTimeKind.Utc).AddTicks(2875));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostDate",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2019, 4, 26, 19, 36, 15, 311, DateTimeKind.Utc).AddTicks(4082),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 4, 16, 18, 45, 0, 456, DateTimeKind.Utc).AddTicks(7539));

            migrationBuilder.CreateTable(
                name: "VotesComments",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Value = table.Column<int>(maxLength: 1, nullable: false, defaultValue: 0),
                    CommentId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotesComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VotesComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VotesComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VotesPosts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Value = table.Column<int>(maxLength: 1, nullable: false, defaultValue: 0),
                    PostId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotesPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VotesPosts_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VotesPosts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VotesComments_CommentId",
                table: "VotesComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_VotesComments_UserId",
                table: "VotesComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VotesPosts_PostId",
                table: "VotesPosts",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_VotesPosts_UserId",
                table: "VotesPosts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VotesComments");

            migrationBuilder.DropTable(
                name: "VotesPosts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Subreddits",
                nullable: false,
                defaultValue: new DateTime(2019, 4, 16, 18, 45, 0, 473, DateTimeKind.Utc).AddTicks(8509),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 4, 26, 19, 36, 15, 406, DateTimeKind.Utc).AddTicks(5269));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostDate",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(2019, 4, 16, 18, 45, 0, 435, DateTimeKind.Utc).AddTicks(2875),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 4, 26, 19, 36, 15, 255, DateTimeKind.Utc).AddTicks(8788));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostDate",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(2019, 4, 16, 18, 45, 0, 456, DateTimeKind.Utc).AddTicks(7539),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 4, 26, 19, 36, 15, 311, DateTimeKind.Utc).AddTicks(4082));
        }
    }
}
