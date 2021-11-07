using Microsoft.EntityFrameworkCore.Migrations;

namespace ELDEL_EntityFramework_Library.Migrations
{
    public partial class ChangeChargersColumnTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SocketType",
                table: "Chargers",
                type: "nvarchar(25)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(15)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ManufacturerType",
                table: "Chargers",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(50)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SocketType",
                table: "Chargers",
                type: "char(15)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ManufacturerType",
                table: "Chargers",
                type: "char(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);
        }
    }
}
