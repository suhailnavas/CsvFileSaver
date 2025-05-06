using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsvFileSaver_WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Mg_New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "CsvEmployeeRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "CsvEmployeeRecords");
        }
    }
}
