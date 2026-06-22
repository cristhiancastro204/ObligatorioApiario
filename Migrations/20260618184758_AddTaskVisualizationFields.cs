using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObligatorioApiario.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskVisualizationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClimaEstado",
                table: "Tareas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClimaTemperatura",
                table: "Tareas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ColmenasRiesgo",
                table: "Tareas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreadoPor",
                table: "Tareas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Tareas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "Tareas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "HerramientasRequeridas",
                table: "Tareas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                name: "NotasCampo",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClimaEstado",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "ClimaTemperatura",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "ColmenasRiesgo",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "HerramientasRequeridas",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "HoraFin",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "HoraInicio",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "NotasCampo",
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
    }
}
