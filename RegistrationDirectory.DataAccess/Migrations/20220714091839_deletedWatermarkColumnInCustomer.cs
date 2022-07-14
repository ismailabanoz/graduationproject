using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistrationDirectory.DataAccess.Migrations
{
    public partial class deletedWatermarkColumnInCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Watermark",
                table: "Customers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Watermark",
                table: "Customers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
