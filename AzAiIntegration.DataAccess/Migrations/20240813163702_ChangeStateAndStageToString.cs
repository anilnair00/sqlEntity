using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirCanada.Appx.AzAiIntegration.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStateAndStageToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestPayload",
                schema: "Integration",
                table: "AzAiIntegration");

            migrationBuilder.DropColumn(
                name: "ResponsePayload",
                schema: "Integration",
                table: "AzAiIntegration");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "Integration",
                table: "AzAiIntegration",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Stage",
                schema: "Integration",
                table: "AzAiIntegration",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "State",
                schema: "Integration",
                table: "AzAiIntegration",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Stage",
                schema: "Integration",
                table: "AzAiIntegration",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestPayload",
                schema: "Integration",
                table: "AzAiIntegration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsePayload",
                schema: "Integration",
                table: "AzAiIntegration",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
