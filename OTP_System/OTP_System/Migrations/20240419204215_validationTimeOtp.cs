using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OTP_System.Migrations
{
    /// <inheritdoc />
    public partial class validationTimeOtp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UsedAt",
                table: "Otps",
                newName: "ExpiryTime");

            migrationBuilder.RenameColumn(
                name: "Counter",
                table: "Otps",
                newName: "validationTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "validationTime",
                table: "Otps",
                newName: "Counter");

            migrationBuilder.RenameColumn(
                name: "ExpiryTime",
                table: "Otps",
                newName: "UsedAt");

            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
