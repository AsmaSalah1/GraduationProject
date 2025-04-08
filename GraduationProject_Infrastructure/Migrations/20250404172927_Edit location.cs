using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Editlocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2ec309ae-c2ee-4847-b349-e96c52a8d0d1", "AQAAAAIAAYagAAAAEE/jb/Qa3sN7Y2Sqb930Xv3Umf37HdST2wXtj/KfQxf9iWHmRx+LLpEPE2JIIV3gNA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b78c3751-b9fd-4f02-aa2c-a5ac1dc54952", "AQAAAAIAAYagAAAAEMLEELH9fyVbeIJ91DZwkTLl75enm5g3iYLrXLZIezCX9P9GhDdf2VZ/hAtnUo4Ktg==" });
        }
    }
}
