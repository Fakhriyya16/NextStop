using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AddedBlogImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaceImages_Places_AttractionId",
                table: "PlaceImages");

            migrationBuilder.RenameColumn(
                name: "AttractionId",
                table: "PlaceImages",
                newName: "PlaceId");

            migrationBuilder.RenameIndex(
                name: "IX_PlaceImages_AttractionId",
                table: "PlaceImages",
                newName: "IX_PlaceImages_PlaceId");

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "PlaceImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BlogImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogId = table.Column<int>(type: "int", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogImages_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogImages_BlogId",
                table: "BlogImages",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceImages_Places_PlaceId",
                table: "PlaceImages",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaceImages_Places_PlaceId",
                table: "PlaceImages");

            migrationBuilder.DropTable(
                name: "BlogImages");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "PlaceImages");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Cities");

            migrationBuilder.RenameColumn(
                name: "PlaceId",
                table: "PlaceImages",
                newName: "AttractionId");

            migrationBuilder.RenameIndex(
                name: "IX_PlaceImages_PlaceId",
                table: "PlaceImages",
                newName: "IX_PlaceImages_AttractionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceImages_Places_AttractionId",
                table: "PlaceImages",
                column: "AttractionId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
