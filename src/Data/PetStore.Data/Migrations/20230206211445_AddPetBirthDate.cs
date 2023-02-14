#nullable disable
namespace PetStore.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddPetBirthDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "AgeInTextFormat",
                table: "Pets");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Pets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Pets");

            migrationBuilder.AddColumn<double>(
                name: "Age",
                table: "Pets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "AgeInTextFormat",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
