using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AshryverBot.Database.Migrations
{
    /// <inheritdoc />
    public partial class TwitchTokenGuidPk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_twitch_tokens",
                table: "twitch_tokens");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "twitch_tokens",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""twitch_tokens"" SET ""Id"" = gen_random_uuid() WHERE ""Id"" IS NULL;");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "twitch_tokens",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_twitch_tokens",
                table: "twitch_tokens",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_twitch_tokens_TwitchUserId",
                table: "twitch_tokens",
                column: "TwitchUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_twitch_tokens",
                table: "twitch_tokens");

            migrationBuilder.DropIndex(
                name: "IX_twitch_tokens_TwitchUserId",
                table: "twitch_tokens");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "twitch_tokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_twitch_tokens",
                table: "twitch_tokens",
                column: "TwitchUserId");
        }
    }
}
