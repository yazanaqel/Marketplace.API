using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.BAL.Migrations
{
    /// <inheritdoc />
    public partial class Images : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalContent",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "ThumbnailContent",
                table: "ProductImages");

            migrationBuilder.RenameColumn(
                name: "OriginalType",
                table: "ProductImages",
                newName: "Folder");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "ProductImages",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Folder",
                table: "ProductImages",
                newName: "OriginalType");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductImages",
                newName: "ImageId");

            migrationBuilder.AddColumn<byte[]>(
                name: "OriginalContent",
                table: "ProductImages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "ProductImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "ThumbnailContent",
                table: "ProductImages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
