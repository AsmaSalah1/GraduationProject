using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Cv", "Email", "EmailConfirmed", "Gender", "GithubLink", "Image", "LinkedInLink", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PersonalExperienceId", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UniversityId", "UserName" },
                values: new object[] { -1, 0, "c092be01-c037-408e-8542-be3db5c29707", null, "mustafaalrifaya3@gmail.com", true, null, null, null, null, false, null, "MUSTAFAALRIFAYA3@GMAIL.COM", "MUSTAFAALRIFAYA", "AQAAAAIAAYagAAAAECMj+2HInt4Ne6vZnF2XpqUdGHaLD2TmZSrL+DQ8o0Qvt0117z/EQAP59mfb8uldnw==", null, null, false, null, false, null, "mustafaalrifaya" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, -1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, -1 });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}
