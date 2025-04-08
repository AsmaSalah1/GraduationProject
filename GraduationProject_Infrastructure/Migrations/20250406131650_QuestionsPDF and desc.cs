using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuestionsPDFanddesc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuestionsPDF",
                table: "Competitions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "No description provided.");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1af20651-b7c1-46fb-91f6-b9bd282f2744", "AQAAAAIAAYagAAAAEIRMzekRoldLiXzcz6UhWZfx+B9V6cT1kDyvsNPjPu/W7bfSk9V+lN3JVYobjDWHjg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionsPDF",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2ec309ae-c2ee-4847-b349-e96c52a8d0d1", "AQAAAAIAAYagAAAAEE/jb/Qa3sN7Y2Sqb930Xv3Umf37HdST2wXtj/KfQxf9iWHmRx+LLpEPE2JIIV3gNA==" });
        }
    }
}
