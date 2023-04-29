#nullable disable
namespace PetStore.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddingPetIdForPetAppointmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PetApppointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "PetApppointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PetApppointments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "PetApppointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetApppointments_IsDeleted",
                table: "PetApppointments",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PetApppointments_IsDeleted",
                table: "PetApppointments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PetApppointments");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "PetApppointments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PetApppointments");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "PetApppointments");
        }
    }
}
