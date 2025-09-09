using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.DAL.Migrations
{
    /// <inheritdoc />
    public partial class initUpdateUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Skills",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Education",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Skills",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
