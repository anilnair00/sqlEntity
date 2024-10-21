﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirCanada.Appx.AzAiIntegration.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveResponseId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseId",
                schema: "Integration",
                table: "AzAiIntegration");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ResponseId",
                schema: "Integration",
                table: "AzAiIntegration",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
