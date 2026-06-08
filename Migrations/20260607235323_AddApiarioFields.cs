using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObligatorioApiario.Migrations
{
    /// <inheritdoc />
    public partial class AddApiarioFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Altitud",
                table: "Apiarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Latitud",
                table: "Apiarios",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitud",
                table: "Apiarios",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Notas",
                table: "Apiarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Zona",
                table: "Apiarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Altitud",
                table: "Apiarios");

            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Apiarios");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Apiarios");

            migrationBuilder.DropColumn(
                name: "Notas",
                table: "Apiarios");

            migrationBuilder.DropColumn(
                name: "Zona",
                table: "Apiarios");
        }
    }
}
