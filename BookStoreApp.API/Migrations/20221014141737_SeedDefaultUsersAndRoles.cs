using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreApp.API.Migrations
{
    public partial class SeedDefaultUsersAndRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "56972b2b-656b-4d41-a21c-7cbd4a177b68", "c0e3a317-f97e-4c56-8800-23b81b0d95c9", "Administrator", "ADMINISTRATOR" },
                    { "ffe01aa8-1231-4f49-bd86-2412a4a00933", "0cc217a6-d754-4c45-99ad-16212b184b90", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "54e1c831-5e3a-4d7a-9dc9-b1773c4739e3", 0, "2654f941-c3fa-4e43-96e3-3a1d09f0eb17", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAEAACcQAAAAEGmMfYUfPRjXQzlMvC4j9Za29BzBVMCMtkGe2paUtYfjYTJqSic08qdheYt9AzmS1A==", null, false, "c93b339f-da9e-4a96-9a6f-f8e9e400248e", false, "user@bookstore.com" },
                    { "56613f0e-5e91-4e9b-b685-932272d6e28a", 0, "b4070187-2291-495b-8365-3a8431b40c9f", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAEAACcQAAAAEBQxdyie8UtVXTQ1unWVXIBAFuRYF2UsmPaBxRcA63uAhGfQBMkiimzWRInVM3fSkQ==", null, false, "a6fa0705-95b4-4d45-a2e9-a0af975fb4f7", false, "admin@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ffe01aa8-1231-4f49-bd86-2412a4a00933", "54e1c831-5e3a-4d7a-9dc9-b1773c4739e3" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "56972b2b-656b-4d41-a21c-7cbd4a177b68", "56613f0e-5e91-4e9b-b685-932272d6e28a" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ffe01aa8-1231-4f49-bd86-2412a4a00933", "54e1c831-5e3a-4d7a-9dc9-b1773c4739e3" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "56972b2b-656b-4d41-a21c-7cbd4a177b68", "56613f0e-5e91-4e9b-b685-932272d6e28a" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "56972b2b-656b-4d41-a21c-7cbd4a177b68");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffe01aa8-1231-4f49-bd86-2412a4a00933");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "54e1c831-5e3a-4d7a-9dc9-b1773c4739e3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "56613f0e-5e91-4e9b-b685-932272d6e28a");
        }
    }
}
