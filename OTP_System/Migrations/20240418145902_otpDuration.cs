using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OTP_System.Migrations
{
    /// <inheritdoc />
    public partial class otpDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OtpDuration",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtpDuration",
                table: "AspNetUsers");
        }
    }
}
