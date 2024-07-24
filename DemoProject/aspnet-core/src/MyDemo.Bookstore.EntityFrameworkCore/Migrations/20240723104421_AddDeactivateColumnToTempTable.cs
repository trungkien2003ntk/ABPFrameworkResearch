using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDemo.BookStore.Migrations
{
    /// <inheritdoc />
    public partial class AddDeactivateColumnToTempTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deactivate",
                table: "AppTemp",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deactivate",
                table: "AppTemp");
        }
    }
}
