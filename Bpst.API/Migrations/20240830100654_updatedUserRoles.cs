using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bpst.API.Migrations
{
    /// <inheritdoc />
    public partial class updatedUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_AppUsers_UserUniqueId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_UserUniqueId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserUniqueId",
                table: "Roles");

            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "AppUsers");

            migrationBuilder.AddColumn<int>(
                name: "UserUniqueId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "UniqueId",
                keyValue: 1,
                column: "UserUniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "UniqueId",
                keyValue: 2,
                column: "UserUniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "UniqueId",
                keyValue: 3,
                column: "UserUniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "UniqueId",
                keyValue: 4,
                column: "UserUniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "UniqueId",
                keyValue: 5,
                column: "UserUniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "UniqueId",
                keyValue: 6,
                column: "UserUniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "UniqueId",
                keyValue: 7,
                column: "UserUniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "UniqueId",
                keyValue: 8,
                column: "UserUniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "UniqueId",
                keyValue: 9,
                column: "UserUniqueId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserUniqueId",
                table: "Roles",
                column: "UserUniqueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_AppUsers_UserUniqueId",
                table: "Roles",
                column: "UserUniqueId",
                principalTable: "AppUsers",
                principalColumn: "UniqueId");
        }
    }
}
