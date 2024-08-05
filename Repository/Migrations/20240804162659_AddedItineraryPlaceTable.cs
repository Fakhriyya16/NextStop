using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AddedItineraryPlaceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_ItineraryDays_ItineraryDayId",
                table: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Places_ItineraryDayId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "ItineraryDayId",
                table: "Places");

            migrationBuilder.CreateTable(
                name: "ItineraryPlace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItineraryDayId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItineraryPlace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItineraryPlace_ItineraryDays_ItineraryDayId",
                        column: x => x.ItineraryDayId,
                        principalTable: "ItineraryDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItineraryPlace_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryPlace_ItineraryDayId",
                table: "ItineraryPlace",
                column: "ItineraryDayId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryPlace_PlaceId",
                table: "ItineraryPlace",
                column: "PlaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItineraryPlace");

            migrationBuilder.AddColumn<int>(
                name: "ItineraryDayId",
                table: "Places",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Places_ItineraryDayId",
                table: "Places",
                column: "ItineraryDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_ItineraryDays_ItineraryDayId",
                table: "Places",
                column: "ItineraryDayId",
                principalTable: "ItineraryDays",
                principalColumn: "Id");
        }
    }
}
