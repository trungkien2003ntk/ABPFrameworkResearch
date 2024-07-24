using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDemo.BookStore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTempTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KindOfFalName",
                table: "AppTemp",
                newName: "Kind_Of_Fal");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppTemp",
                type: "nvarchar(40)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Kind_Of_Fal",
                table: "AppTemp",
                newName: "KindOfFalName");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppTemp",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldNullable: true);
        }
    }
}
