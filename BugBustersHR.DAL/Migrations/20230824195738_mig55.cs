using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugBustersHR.DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig55 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2d55fd9c-f818-4672-9bad-77a463821171", "1eed7527-4d48-4e1b-a055-2805f01b6c80" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c8c708ee-4567-4a2c-9bfe-ab7bc691a5a9", "59a25eea-63cc-485d-b538-6807b3aa545c" });
        }
    }
}
