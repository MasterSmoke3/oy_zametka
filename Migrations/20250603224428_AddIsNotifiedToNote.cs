using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZametkiApp.Migrations
{
    /// <inheritdoc />
    public partial class AddIsNotifiedToNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNotified",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNotified",
                table: "Notes");
        }
    }
}
