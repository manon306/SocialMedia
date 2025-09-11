using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.DAL.Migrations
{
    /// <inheritdoc />
    public partial class initAddBlockedtoConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Connections",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Connections");
        }
    }
}
