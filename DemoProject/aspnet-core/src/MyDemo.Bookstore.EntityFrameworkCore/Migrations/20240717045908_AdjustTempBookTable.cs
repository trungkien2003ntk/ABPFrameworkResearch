using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDemo.BookStore.Migrations
{
    /// <inheritdoc />
    public partial class AdjustTempBookTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppBooksTemp_AppBooks_Id",
                table: "AppBooksTemp");

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "AppBooksTemp",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "AppBooksTemp",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "AppBooksTemp",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "AppBooksTemp",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "AppBooksTemp",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "AppBooksTemp",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "AppBooksTemp",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AppBooksTemp",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "AppBooksTemp",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "AppBooksTemp",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "AppBooksTemp",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "AppBooksTemp");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "AppBooksTemp");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AppBooksTemp");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "AppBooksTemp");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "AppBooksTemp");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "AppBooksTemp");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "AppBooksTemp");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AppBooksTemp");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "AppBooksTemp");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "AppBooksTemp");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AppBooksTemp");

            migrationBuilder.AddForeignKey(
                name: "FK_AppBooksTemp_AppBooks_Id",
                table: "AppBooksTemp",
                column: "Id",
                principalTable: "AppBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
