using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleManagementAPI.Migrations
{
    public partial class LoginAndRegister : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "VehicleOwners");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "VehicleOwners",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "VehicleOwners",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "VehicleOwners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "VehicleOwners");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "VehicleOwners");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "VehicleOwners");

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "VehicleOwners",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
