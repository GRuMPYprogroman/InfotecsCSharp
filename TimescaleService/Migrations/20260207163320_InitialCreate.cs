using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TimescaleService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    FileName = table.Column<string>(type: "text", nullable: false),
                    DateDelta = table.Column<int>(type: "integer", nullable: false),
                    MinimumDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AverageExecTime = table.Column<double>(type: "double precision", nullable: false),
                    AverageValue = table.Column<double>(type: "double precision", nullable: false),
                    MedianValue = table.Column<double>(type: "double precision", nullable: false),
                    MaximumValue = table.Column<double>(type: "double precision", nullable: false),
                    MinimumValue = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.FileName);
                });

            migrationBuilder.CreateTable(
                name: "Values",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExecutionTime = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Values");
        }
    }
}
