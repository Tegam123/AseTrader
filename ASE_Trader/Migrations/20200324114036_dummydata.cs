using Microsoft.EntityFrameworkCore.Migrations;

namespace ASE_Trader.Migrations
{
    public partial class dummydata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AccountType", "Email", "FirstName", "LastName", "PwHash" },
                values: new object[] { 1L, 0, "vk@hotmail.com", "Victor", "Kildahl", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AccountType", "Email", "FirstName", "LastName", "PwHash" },
                values: new object[] { 2L, 0, "lm@hotmail.com", "Lasse", "Mosel", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AccountType", "Email", "FirstName", "LastName", "PwHash" },
                values: new object[] { 3L, 0, "dt@hotmail.com", "David", "Tegam", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3L);
        }
    }
}
