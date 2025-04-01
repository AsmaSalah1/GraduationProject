using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPostTypeToPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Posttype",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Posttype",
                table: "Posts");
        }
    }
}
