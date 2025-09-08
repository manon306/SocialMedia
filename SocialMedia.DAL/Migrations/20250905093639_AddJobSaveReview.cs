using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddJobSaveReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSaved",
                table: "Jobs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSaved",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "Jobs");
        }
    }
}
