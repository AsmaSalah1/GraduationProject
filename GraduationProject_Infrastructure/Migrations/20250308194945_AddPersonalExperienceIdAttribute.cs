using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonalExperienceIdAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonalExperienceId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalExperienceId",
                table: "AspNetUsers");
        }
    }
}
