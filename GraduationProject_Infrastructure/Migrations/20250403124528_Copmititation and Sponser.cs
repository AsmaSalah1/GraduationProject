using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CopmititationandSponser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image2",
                table: "Competitions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "ConcurrencyStamp", "Gender", "PasswordHash" },
                values: new object[] { "b78c3751-b9fd-4f02-aa2c-a5ac1dc54952", 0, "AQAAAAIAAYagAAAAEMLEELH9fyVbeIJ91DZwkTLl75enm5g3iYLrXLZIezCX9P9GhDdf2VZ/hAtnUo4Ktg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image2",
                table: "Competitions");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "ConcurrencyStamp", "Gender", "PasswordHash" },
                values: new object[] { "c092be01-c037-408e-8542-be3db5c29707", null, "AQAAAAIAAYagAAAAECMj+2HInt4Ne6vZnF2XpqUdGHaLD2TmZSrL+DQ8o0Qvt0117z/EQAP59mfb8uldnw==" });
        }
    }
}
