using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsvFileSaver_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Mg_fileupload_2_action_Removed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "UploadFileDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "UploadFileDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
