using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistrationDirectory.DataAccess.Migrations
{
    public partial class addedTokenAndRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Guid = table.Column<string>(type: "text", nullable: false),
                    ExpDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.UserName);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");
        }
    }
}
