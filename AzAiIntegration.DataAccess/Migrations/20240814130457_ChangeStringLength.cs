using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirCanada.Appx.AzAiIntegration.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStringLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "Integration",
                table: "AzAiIntegration",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: null,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Stage",
                schema: "Integration",
                table: "AzAiIntegration",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: null,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
              name: "State",
                schema: "Integration",
                table: "AzAiIntegration",
                type: "nvarchar(max)",
                maxLength: null,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Stage",
                schema: "Integration",
                table: "AzAiIntegration",
                type: "nvarchar(max)",
                maxLength: null,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
