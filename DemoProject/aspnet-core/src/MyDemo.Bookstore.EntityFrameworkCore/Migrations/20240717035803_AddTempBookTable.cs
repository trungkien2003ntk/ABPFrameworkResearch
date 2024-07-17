using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDemo.BookStore.Migrations
{
    /// <inheritdoc />
    public partial class AddTempBookTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppBooksTemp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppBooksTemp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppBooksTemp_AppBooks_Id",
                        column: x => x.Id,
                        principalTable: "AppBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppBooksTemp");
        }
    }
}
