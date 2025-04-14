using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ForgienKeyinteamtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "72df538a-d090-4221-8904-f743e876d01b", "AQAAAAIAAYagAAAAEPhVWrnVTGy1xJ8pT0vDttgxiI2iCV0AS7aiAjw+MWo/0WtbCaIEQgIbL39GumQP/Q==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Teams");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "076649cf-0d34-4306-a9d8-086c7d0dae7f", "AQAAAAIAAYagAAAAENSW8RKk9JIuFJT269zOwHI5zdzux+9BcEsTTDMn7+O9iN+YBjwKF/LbRri61F47Kw==" });
        }
    }
}
