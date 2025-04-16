using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsvFileSaver_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Mg_fileupload_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "UploadFileDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "UploadFileDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "UploadFileDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "UploadFileDetails");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "UploadFileDetails");

            migrationBuilder.DropColumn(
                name: "status",
                table: "UploadFileDetails");
        }
    }
}
