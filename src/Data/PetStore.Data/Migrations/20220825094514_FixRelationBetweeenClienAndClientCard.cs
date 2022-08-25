using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetStore.Data.Migrations
{
    public partial class FixRelationBetweeenClienAndClientCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientCards_AspNetUsers_ClientId",
                table: "ClientCards");

            migrationBuilder.DropIndex(
                name: "IX_ClientCards_ClientId",
                table: "ClientCards");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClientCardId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "ClientCards",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClientCardId",
                table: "AspNetUsers",
                column: "ClientCardId",
                unique: true,
                filter: "[ClientCardId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClientCardId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "ClientCards",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ClientCards_ClientId",
                table: "ClientCards",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClientCardId",
                table: "AspNetUsers",
                column: "ClientCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCards_AspNetUsers_ClientId",
                table: "ClientCards",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
