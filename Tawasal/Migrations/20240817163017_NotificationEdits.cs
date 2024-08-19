using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tawasal.Migrations
{
    /// <inheritdoc />
    public partial class NotificationEdits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferenceId",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notifications");
        }
    }
}
