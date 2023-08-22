using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugBustersHR.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ınıtj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "90323c5c-ceb3-4f6e-969e-276be2eb36c5", "30d252f0-de09-4634-80b7-24905b5d7d46" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "fa16bab6-83c8-41c2-b18e-05d17d2ce33b", "2e43dc94-9f54-4a82-9e24-a5aa3a3aca30" });
        }
    }
}
