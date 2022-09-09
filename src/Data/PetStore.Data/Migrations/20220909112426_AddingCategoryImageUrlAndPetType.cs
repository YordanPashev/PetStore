#nullable disable
namespace PetStore.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddingCategoryImageUrlAndPetType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Categories_CategoryId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_CategoryId",
                table: "Pets");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Pets",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Pets",
                newName: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_CategoryId",
                table: "Pets",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Categories_CategoryId",
                table: "Pets",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
