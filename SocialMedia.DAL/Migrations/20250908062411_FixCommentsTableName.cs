using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixCommentsTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Posts_PostID",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Reacts_Comment_CommentID",
                table: "Reacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reply_Comment_ParentCommentID",
                table: "Reply");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_PostID",
                table: "Comments",
                newName: "IX_Comments_PostID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostID",
                table: "Comments",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reacts_Comments_CommentID",
                table: "Reacts",
                column: "CommentID",
                principalTable: "Comments",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reply_Comments_ParentCommentID",
                table: "Reply",
                column: "ParentCommentID",
                principalTable: "Comments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reacts_Comments_CommentID",
                table: "Reacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reply_Comments_ParentCommentID",
                table: "Reply");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PostID",
                table: "Comment",
                newName: "IX_Comment_PostID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Posts_PostID",
                table: "Comment",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reacts_Comment_CommentID",
                table: "Reacts",
                column: "CommentID",
                principalTable: "Comment",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reply_Comment_ParentCommentID",
                table: "Reply",
                column: "ParentCommentID",
                principalTable: "Comment",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
