using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OTP_System.Migrations
{
    /// <inheritdoc />
    public partial class SecretKeyUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "OTPs");

            migrationBuilder.AddColumn<long>(
                name: "timestamp",
                table: "OTPs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "timestamp",
                table: "OTPs");

            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "OTPs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
