using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
