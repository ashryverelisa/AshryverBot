using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AshryverBot.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddWatchtime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "watchtimes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TwitchUserId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Login = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    TotalSeconds = table.Column<long>(type: "bigint", nullable: false),
                    LastSeenAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_watchtimes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_watchtimes_TwitchUserId",
                table: "watchtimes",
                column: "TwitchUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "watchtimes");
        }
    }
}
