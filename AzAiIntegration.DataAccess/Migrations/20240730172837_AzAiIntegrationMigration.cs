using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirCanada.Appx.AzAiIntegration.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AzAiIntegrationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Integration");

            migrationBuilder.CreateTable(
                name: "AzAiIntegration",
                schema: "Integration",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RequestMsg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseId = table.Column<long>(type: "bigint", nullable: true),
                    ResponseMsg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SsetDocumentId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    MessageMetaData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestPayload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponsePayload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stage = table.Column<int>(type: "int", nullable: true),
                    State = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AzAiIntegration", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AzAiIntegration",
                schema: "Integration");
        }
    }
}
