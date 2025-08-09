using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookRadar.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistorialBusquedas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Autor = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AnioPublicacion = table.Column<int>(type: "int", nullable: true),
                    Editorial = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaConsulta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialBusquedas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistorialBusquedas");
        }
    }
}
