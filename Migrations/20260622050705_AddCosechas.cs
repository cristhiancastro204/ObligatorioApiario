using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObligatorioApiario.Migrations
{
    /// <inheritdoc />
    public partial class AddCosechas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApiarioId",
                table: "Tareas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cosechas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiarioId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notas = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cosechas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cosechas_Apiarios_ApiarioId",
                        column: x => x.ApiarioId,
                        principalTable: "Apiarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CosechasColmenas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CosechaId = table.Column<int>(type: "int", nullable: false),
                    ColmenaId = table.Column<int>(type: "int", nullable: false),
                    CantidadKg = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CosechasColmenas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CosechasColmenas_Colmenas_ColmenaId",
                        column: x => x.ColmenaId,
                        principalTable: "Colmenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CosechasColmenas_Cosechas_CosechaId",
                        column: x => x.CosechaId,
                        principalTable: "Cosechas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_ApiarioId",
                table: "Tareas",
                column: "ApiarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Cosechas_ApiarioId",
                table: "Cosechas",
                column: "ApiarioId");

            migrationBuilder.CreateIndex(
                name: "IX_CosechasColmenas_ColmenaId",
                table: "CosechasColmenas",
                column: "ColmenaId");

            migrationBuilder.CreateIndex(
                name: "IX_CosechasColmenas_CosechaId",
                table: "CosechasColmenas",
                column: "CosechaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_Apiarios_ApiarioId",
                table: "Tareas",
                column: "ApiarioId",
                principalTable: "Apiarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_Apiarios_ApiarioId",
                table: "Tareas");

            migrationBuilder.DropTable(
                name: "CosechasColmenas");

            migrationBuilder.DropTable(
                name: "Cosechas");

            migrationBuilder.DropIndex(
                name: "IX_Tareas_ApiarioId",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "ApiarioId",
                table: "Tareas");
        }
    }
}
