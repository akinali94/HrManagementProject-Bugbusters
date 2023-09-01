using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugBustersHR.DAL.Migrations
{
    /// <inheritdoc />
    public partial class newdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "0f6805a6-f83d-4b95-a248-7756edf0669b", "771aeda8-c47e-4397-ad24-fe6cf3d731c6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d02ec6e6-03ac-476f-abab-cf1afb0b8c18", "08a144cb-ab6b-4c08-9693-f67fe276991f" });
        }
    }
}
