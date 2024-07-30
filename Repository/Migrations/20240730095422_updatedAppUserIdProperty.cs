using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class updatedAppUserIdProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Subscriptions_SubscriptionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_AspNetUsers_AppUserId1",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_AppUserId1",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Itineraries_AspNetUsers_AppUserId1",
                table: "Itineraries");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_AppUserId1",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_AppUserId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_AppUserId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Payments_AppUserId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Itineraries_AppUserId1",
                table: "Itineraries");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_AppUserId1",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_AppUserId1",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SubscriptionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "Itineraries");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Subscriptions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Itineraries",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Favorites",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_AppUserId",
                table: "Subscriptions",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AppUserId",
                table: "Reviews",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AppUserId",
                table: "Payments",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_AppUserId",
                table: "Itineraries",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_AppUserId",
                table: "Favorites",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_AppUserId",
                table: "Blogs",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AspNetUsers_AppUserId",
                table: "Blogs",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_AppUserId",
                table: "Favorites",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Itineraries_AspNetUsers_AppUserId",
                table: "Itineraries",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_AppUserId",
                table: "Payments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_AppUserId",
                table: "Reviews",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AspNetUsers_AppUserId",
                table: "Subscriptions",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_AspNetUsers_AppUserId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_AppUserId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Itineraries_AspNetUsers_AppUserId",
                table: "Itineraries");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_AppUserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_AppUserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AspNetUsers_AppUserId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_AppUserId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_AppUserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Payments_AppUserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Itineraries_AppUserId",
                table: "Itineraries");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_AppUserId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_AppUserId",
                table: "Blogs");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Payments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Itineraries",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "Itineraries",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Favorites",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "Favorites",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Blogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AppUserId1",
                table: "Reviews",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AppUserId1",
                table: "Payments",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_AppUserId1",
                table: "Itineraries",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_AppUserId1",
                table: "Favorites",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_AppUserId1",
                table: "Blogs",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SubscriptionId",
                table: "AspNetUsers",
                column: "SubscriptionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Subscriptions_SubscriptionId",
                table: "AspNetUsers",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AspNetUsers_AppUserId1",
                table: "Blogs",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_AppUserId1",
                table: "Favorites",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Itineraries_AspNetUsers_AppUserId1",
                table: "Itineraries",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_AppUserId1",
                table: "Payments",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_AppUserId1",
                table: "Reviews",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
