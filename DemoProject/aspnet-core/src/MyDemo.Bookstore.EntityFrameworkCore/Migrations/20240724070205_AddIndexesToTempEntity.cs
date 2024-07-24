using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDemo.BookStore.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesToTempEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "AppTemp",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppTemp_Discriminator",
                table: "AppTemp",
                column: "Discriminator");

            migrationBuilder.CreateIndex(
                name: "IX_AppTemp_ImportId",
                table: "AppTemp",
                column: "ImportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppTemp_Discriminator",
                table: "AppTemp");

            migrationBuilder.DropIndex(
                name: "IX_AppTemp_ImportId",
                table: "AppTemp");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "AppTemp",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
