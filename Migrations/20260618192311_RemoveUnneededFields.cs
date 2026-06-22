using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObligatorioApiario.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnneededFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoraFin",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "HoraInicio",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "Responsable",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "ResponsableCargo",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "SectorColmenas",
                table: "Tareas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HoraFin",
                table: "Tareas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HoraInicio",
                table: "Tareas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Responsable",
                table: "Tareas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResponsableCargo",
                table: "Tareas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SectorColmenas",
                table: "Tareas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
