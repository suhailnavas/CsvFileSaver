using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsvFileSaver_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Mg_5_RoleAndUserName_UserID_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UploadFileDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UploadFileDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UploadFileDetails");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UploadFileDetails");
        }
    }
}
